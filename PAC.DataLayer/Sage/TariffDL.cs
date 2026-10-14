using PAC.Common.Sage;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;

namespace PAC.DataLayer.Sage
{
    public class TariffDL : BaseDL
    {
        #region GetMinTariffs
        public static List<Tariff> GetMinTariffs(string custno)
        {
            List<Tariff> list = new List<Tariff>();
            Tariff item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT MIN(unitpr1) as minprice, origin, dest, currid FROM ICPRIC01 " +
                    "WHERE custno = '" + custno + "' " +
                    "AND unitpr1 > 0 " +
                    "GROUP BY origin, dest, currid";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new Tariff();

                        item.CustNo = custno.Trim();

                        if (reader["minprice"] != (object)DBNull.Value)
                            item.UnitPrice = Convert.ToDecimal(reader["minprice"].ToString());

                        if (reader["currid"] != (object)DBNull.Value)
                            item.Curreny = reader["currid"].ToString().Trim();

                        if (reader["origin"] != (object)DBNull.Value)
                            item.Origin = reader["origin"].ToString().Trim();

                        if (reader["dest"] != (object)DBNull.Value)
                            item.Destination = reader["dest"].ToString().Trim();

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

        #region GetTariffsByOriginDest
        public static List<Tariff> GetTariffsByOriginDest(string custno, string origin, string dest)
        {
            List<Tariff> list = new List<Tariff>();
            Tariff item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT distinct p.item, i.itmdesc, p.custno, p.begqty1, p.endqty1, p.unitpr1, p.begqty2, p.endqty2, p.unitpr2, p.currid, p.serv_level, " +
                    "p.origin, p.dest " +
                    "FROM ICPRIC01 p " +
                    "LEFT JOIN icitem01 i on i.Item = p.item " +
                    "WHERE custno = '" + custno + "' " +
                    "and origin = '" + origin + "' " +
                    "and dest = '" + dest + "' " +
                    "AND unitpr1 > 0 " +
                    "AND i.itemstat = 'A' " +
                    "ORDER BY p.item, p.serv_level ";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new Tariff();

                        if (reader["custno"] != (object)DBNull.Value)
                            item.CustNo = reader["custno"].ToString().Trim();

                        if (reader["item"] != (object)DBNull.Value)
                            item.Item = reader["item"].ToString().Trim();

                        if (reader["itmdesc"] != (object)DBNull.Value)
                            item.ItemDescription = reader["itmdesc"].ToString().Trim();

                        if (reader["begqty1"] != (object)DBNull.Value)
                            item.BeginQty = Convert.ToDecimal(reader["begqty1"].ToString());

                        if (reader["endqty1"] != (object)DBNull.Value)
                            item.EndQty = Convert.ToDecimal(reader["endqty1"].ToString());

                        if (reader["unitpr1"] != (object)DBNull.Value)
                            item.UnitPrice = Convert.ToDecimal(reader["unitpr1"].ToString());

                        if (reader["begqty2"] != (object)DBNull.Value)
                            item.BeginQty2 = Convert.ToDecimal(reader["begqty1"].ToString());

                        if (reader["endqty2"] != (object)DBNull.Value)
                            item.EndQty2 = Convert.ToDecimal(reader["endqty1"].ToString());

                        if (reader["unitpr2"] != (object)DBNull.Value)
                            item.UnitPrice2 = Convert.ToDecimal(reader["unitpr1"].ToString());

                        if (reader["currid"] != (object)DBNull.Value)
                            item.Curreny = reader["currid"].ToString().Trim();

                        if (reader["serv_level"] != (object)DBNull.Value)
                            item.ServiceLevel = reader["serv_level"].ToString().Trim();

                        if (reader["origin"] != (object)DBNull.Value)
                            item.Origin = reader["origin"].ToString().Trim();

                        if (reader["dest"] != (object)DBNull.Value)
                            item.Destination = reader["dest"].ToString().Trim();

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
