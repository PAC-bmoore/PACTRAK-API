using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PAC.Common.Sage;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace PAC.DataLayer.Sage
{
    public class InvoiceDL : BaseDL
    {
        #region GetInvoicesByCustDateRange
        public static List<Invoice> GetInvoicesByCustDateRange(string customerNum, DateTime minDate, DateTime maxDate)
        {
            List<Invoice> list = new List<Invoice>();
            Invoice item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string minDateString = string.Format("datetime({0},{1},{2})", minDate.Year, minDate.Month, minDate.Day);
                string maxDateString = string.Format("datetime({0},{1},{2})", maxDate.Year, maxDate.Month, maxDate.Day);

                string query = "SELECT custno, Invno, Invdte, ponum, refno, taxrate, tax, invamt, paidamt, balance, pterms, Dtepaid, ordate, ornum, exchrat " +
                    "FROM armast01 " +
                    "WHERE custno = '" + customerNum + "' " +
                    "AND invdte >= " + minDateString + " " +
                    "AND invdte <= " + maxDateString + " " +
                    "ORDER BY invdte desc";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new Invoice();

                        if (reader["ornum"] != (object)DBNull.Value)
                            item.Sono = reader["ornum"].ToString();

                        if (reader["Invno"] != (object)DBNull.Value)
                            item.InvoiceNumber = reader["Invno"].ToString();

                        if (reader["Invdte"] != (object)DBNull.Value)
                        {
                            item.InvoiceDate = reader.GetDateTime(reader.GetOrdinal("Invdte"));
                            item.InvoiceDueDate = item.InvoiceDueDate.AddDays(30);
                        }

                        if (reader["invamt"] != (object)DBNull.Value)
                            item.InvoiceTotal = reader.GetDecimal(reader.GetOrdinal("invamt"));

                        if (reader["paidamt"] != (object)DBNull.Value)
                            item.AmountPaid = reader.GetDecimal(reader.GetOrdinal("paidamt"));

                        if (reader["balance"] != (object)DBNull.Value)
                            item.Balance = reader.GetDecimal(reader.GetOrdinal("balance"));

                        if (reader["Dtepaid"] != (object)DBNull.Value)
                            item.PaidDate = reader.GetDateTime(reader.GetOrdinal("Dtepaid"));

                        if (item.Balance == 0)
                            item.InvoiceStatus = "Paid";
                        else if (item.Balance > 0 && DateTime.Now > item.InvoiceDueDate)
                            item.InvoiceStatus = "Overdue";
                        else
                            item.InvoiceStatus = "Due";

                        list.Add(item);
                    }
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                conn.Close();
            }

            return list;
        }
        #endregion

        #region GetInvoiceBySONO
        public static Invoice GetInvoiceBySONO(string sono, string invoiceNum)
        {
            Invoice item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT A.INVNO as Air_Waybill, S.ORIGIN as Origin, S.DEST as Destination, S.PIECES as Total_Pieces_Shipped, " +
                    "S.WEIGHT as Actual_Weight, S.WEIGHT as Chargeable_Weight, S.POOFKGS as Weight_Unit, A.INVDTE as Invoice_Date, " +
                    "A.INVAMT as Invoice_Amount, S.SONO, A.ORDATE, a.tax, s.serv_level,s.Ftype,s.pterms, s.ponum, s.custno " +
                    "FROM ARMAST01 A " +
                    "LEFT JOIN SOMAST01 S ON S.SONO = A.ORNUM " +
                    "Where ALLTRIM(S.SONO) = '" + sono + "' " +
                    "AND ALLTRIM(A.INVNO) = '" + invoiceNum + "' " +
                    "ORDER BY S.SONO DESC";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    if (reader.Read())
                    {
                        item = new Invoice();
/*
                        if (reader["custno"] != (object)DBNull.Value)
                            item.Customer = reader["custno"].ToString().Trim();

                        if (reader["Air_Waybill"] != (object)DBNull.Value)
                            item.AirWayBillNum = reader["Air_Waybill"].ToString().Trim();

                        if (reader["Origin"] != (object)DBNull.Value)
                            item.Origin = reader["Origin"].ToString().Trim();

                        if (reader["Destination"] != (object)DBNull.Value)
                            item.Destination = reader["Destination"].ToString().Trim();

                        if (reader["Total_Pieces_Shipped"] != (object)DBNull.Value)
                            item.TotalPiecesShipped = Convert.ToInt32(reader["Total_Pieces_Shipped"].ToString());

                        if (reader["Actual_Weight"] != (object)DBNull.Value)
                            item.ActualWeight = Convert.ToDecimal(reader["Actual_Weight"].ToString());

                        if (reader["Chargeable_Weight"] != (object)DBNull.Value)
                            item.ChargeableWeight = Convert.ToDecimal(reader["Chargeable_Weight"].ToString());

                        if (reader["Weight_Unit"] != (object)DBNull.Value)
                            item.UnitOfMeasure = reader["Weight_Unit"].ToString().Trim();

                        if (reader["Invoice_Date"] != (object)DBNull.Value)
                            item.InvoiceDate = reader.GetDateTime(reader.GetOrdinal("Invoice_Date"));

                        if (reader["Invoice_Amount"] != (object)DBNull.Value)
                            item.InvoiceTotal = reader.GetDecimal(reader.GetOrdinal("Invoice_Amount"));

                        if (reader["tax"] != (object)DBNull.Value)
                            item.InvoiceTax = reader.GetDecimal(reader.GetOrdinal("tax"));

                        if (reader["SONO"] != (object)DBNull.Value)
                            item.SalesOrderNumber = reader["SONO"].ToString().Trim();

                        if (reader["ORDATE"] != (object)DBNull.Value)
                            item.OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDATE"));

                        if (reader["serv_level"] != (object)DBNull.Value)
                            item.ServiceLevel = reader["serv_level"].ToString().Trim();

                        if (reader["Ftype"] != (object)DBNull.Value)
                            item.FreightType = reader["Ftype"].ToString().Trim();

                        if (reader["pterms"] != (object)DBNull.Value)
                            item.PaymentTerms = reader["pterms"].ToString().Trim();

                        if (reader["ponum"] != (object)DBNull.Value)
                            item.SpotNumber = reader["ponum"].ToString().Trim();*/
                    }
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                conn.Close();
            }

            return item;
        }
        #endregion

        #region GetInvoiceTransactions
        public static List<InvoiceItem> GetInvoiceTransactions(string invNo, string custNo)
        {
            List<InvoiceItem> list = new List<InvoiceItem>();
            InvoiceItem item = new InvoiceItem();
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT invno,item,custno,descrip,umeasur,price,qtyord,qtyshp,invdte,ponum,extprice,transeq,tranlineno,sono " +
                    "FROM artran01 " +
                    "WHERE ALLTRIM(invno) = '" + invNo + "' " +
                    "AND ALLTRIM(custno) = '" + custNo + "' ";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new InvoiceItem();

                        if (reader["item"] != (object)DBNull.Value)
                            item.Item = reader["item"].ToString().Trim();

                        if (reader["descrip"] != (object)DBNull.Value)
                            item.Description = reader["descrip"].ToString().Trim();

                        if (reader["price"] != (object)DBNull.Value)
                            item.Price = reader.GetDecimal(reader.GetOrdinal("price"));

                        if (reader["qtyord"] != (object)DBNull.Value)
                            item.Quantity = reader.GetDecimal(reader.GetOrdinal("qtyord"));

                        if (reader["extprice"] != (object)DBNull.Value)
                            item.Total = reader.GetDecimal(reader.GetOrdinal("extprice"));

                        list.Add(item);
                    }
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                conn.Close();
            }

            return list;
        }
        #endregion

        #region GetInvoicesBySONOwithPOD
        public static List<InvoiceDetail> GetInvoicesBySONOwithPOD(string sono, string invoiceNum)
        {
            List<InvoiceDetail> list = new List<InvoiceDetail>();
            InvoiceDetail item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT A.INVNO as Air_Waybill, S.ORIGIN as Origin, S.DEST as Destination, S.PIECES as Total_Pieces_Shipped, " +
                    "S.WEIGHT as Actual_Weight, S.WEIGHT as Chargeable_Weight, S.POOFKGS as Weight_Unit, A.INVDTE as Invoice_Date, " +
                    "A.INVAMT as Invoice_Amount, S.SONO, A.ORDATE, S.trndte, S.trnto, S.trneta " +
                    "FROM ARMAST01 A " +
                    "LEFT JOIN SOMAST01 S ON S.SONO = A.ORNUM " +
                    "Where ALLTRIM(S.SONO) = '" + sono + "' " +
                    "AND ALLTRIM(A.INVNO) = '" + invoiceNum + "' " +
                    "ORDER BY S.SONO DESC";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new InvoiceDetail();

                        if (reader["Air_Waybill"] != (object)DBNull.Value)
                            item.AWB = reader["Air_Waybill"].ToString().Trim();

                        if (reader["Origin"] != (object)DBNull.Value)
                            item.Origin = reader["Origin"].ToString().Trim();

                        if (reader["Destination"] != (object)DBNull.Value)
                            item.Destination = reader["Destination"].ToString().Trim();

                        if (reader["Total_Pieces_Shipped"] != (object)DBNull.Value)
                            item.TotalPiecesShipped = Convert.ToInt32(reader["Total_Pieces_Shipped"].ToString());

                        if (reader["Actual_Weight"] != (object)DBNull.Value)
                            item.ActualWeight = Convert.ToDecimal(reader["Actual_Weight"].ToString());

                        if (reader["Chargeable_Weight"] != (object)DBNull.Value)
                            item.ChargeableWeight = Convert.ToDecimal(reader["Chargeable_Weight"].ToString());

                        if (reader["Weight_Unit"] != (object)DBNull.Value)
                            item.UnitOfMeasure = reader["Weight_Unit"].ToString().Trim();

                        if (reader["Invoice_Date"] != (object)DBNull.Value)
                            item.InvoiceDate = reader.GetDateTime(reader.GetOrdinal("Invoice_Date"));

                        if (reader["Invoice_Amount"] != (object)DBNull.Value)
                            item.InvoiceTotal = reader.GetDecimal(reader.GetOrdinal("Invoice_Amount"));

                        if (reader["SONO"] != (object)DBNull.Value)
                            item.SalesOrderNumber = reader["SONO"].ToString().Trim();

                        if (reader["ORDATE"] != (object)DBNull.Value)
                            item.OrderDate = reader.GetDateTime(reader.GetOrdinal("ORDATE"));

                        if (reader["trndte"] != (object)DBNull.Value)
                            item.TansferDate = reader.GetDateTime(reader.GetOrdinal("trndte"));

                        if (reader["trnto"] != (object)DBNull.Value)
                            item.TansferTo = reader["trnto"].ToString().Trim();

                        if (reader["trneta"] != (object)DBNull.Value)
                            item.TansferETA = reader["trneta"].ToString().Trim();

                        item.POD = PODDL.GetPODInformation(item.AWB);
                        list.Add(item);
                    }
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                conn.Close();
            }

            return list;
        }
        #endregion

    }
}
