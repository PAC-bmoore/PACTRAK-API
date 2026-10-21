using PAC.Common.Sage;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Reflection;
using System.Text;

namespace PAC.DataLayer.Sage
{
    public class PODDL : BaseDL
    {
        #region GetRecentPODHistory
        public static List<PODHistoryItem> GetPODHistory(string custoNo, DateTime minDate, DateTime maxDate)
        {
            List<PODHistoryItem> list = new List<PODHistoryItem>();
            PODHistoryItem item = new PODHistoryItem();
            OleDbConnection conn = null;

            try
            {
                int ctr = 0;
                string minDateString = string.Format("datetime({0},{1},{2})", minDate.Year, minDate.Month, minDate.Day);
                string maxDateString = string.Format("datetime({0},{1},{2})", maxDate.Year, maxDate.Month, maxDate.Day);

                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT distinct So.SODATE, So.ORDATE, So.ORIGIN, So.DEST, So.PIECES, So.AWBPREFIX, So.SONO, So.WEIGHT, So.WTSHIPPED, So.POOFKGS, PO.RECPCS, " +
                    "PO.SignBy, pickdate, PO.picktime, So.PCShipped, So.trndte, So.trnto, So.trneta, m.fltdate, m.fltno " +
                    "FROM SOMAST01 So " +
                    "LEFT JOIN SOPODD01 PO ON PO.AWBNO = So.SONO " +
                    "LEFT JOIN somanf01 m ON m.sono = So.sono " +
                    "WHERE ((So.SOSTAT = 'C') OR (So.WtShipped > 0  OR  So.PCShipped > 0)) " +
                    "AND ALLTRIM(So.CUSTNO) = '" + custoNo + "' " +
                    "AND so.ORDATE >= " + minDateString + " " +
                    "AND so.ORDATE <= " + maxDateString + " " +
                    "AND (m.lnstat is null OR m.lnstat != 'X') " +
                    "ORDER BY So.ORDATE desc";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new PODHistoryItem();

                        if (reader["SODATE"] != (object)DBNull.Value)
                            item.DateTendered = reader.GetDateTime(reader.GetOrdinal("SODATE"));

                        if (reader["fltdate"] != (object)DBNull.Value)
                            item.FlightDate = reader.GetDateTime(reader.GetOrdinal("fltdate"));

                        if (reader["fltno"] != (object)DBNull.Value)
                            item.FlightNumber = reader["fltno"].ToString().Trim();

                        if (reader["ORIGIN"] != (object)DBNull.Value)
                            item.Origin = reader["ORIGIN"].ToString().Trim();

                        if (reader["DEST"] != (object)DBNull.Value)
                            item.Destination = reader["DEST"].ToString().Trim();

                        if (reader["PIECES"] != (object)DBNull.Value)
                            item.PiecesTendered = Convert.ToInt32(reader["PIECES"].ToString());

                        if (reader["AWBPREFIX"] != (object)DBNull.Value)
                            item.AWBPrefix = reader["AWBPREFIX"].ToString().Trim();

                        if (reader["SONO"] != (object)DBNull.Value)
                            item.AWBNumber = reader["SONO"].ToString().Trim();

                        if (reader["WEIGHT"] != (object)DBNull.Value)
                            item.WeightTendered = Convert.ToDecimal(reader["WEIGHT"].ToString());

                        if (reader["WTSHIPPED"] != (object)DBNull.Value)
                            item.WeightShipped = Convert.ToDecimal(reader["WTSHIPPED"].ToString());

                        if (reader["POOFKGS"] != (object)DBNull.Value)
                            item.WeightUnit = reader["POOFKGS"].ToString().Trim();

                        if (reader["RECPCS"] != (object)DBNull.Value)
                            item.PiecesPickedUp = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("RECPCS")));

                        if (reader["SignBy"] != (object)DBNull.Value)
                            item.SignedBy = reader["SignBy"].ToString().Trim();

                        if (reader["pickdate"] != (object)DBNull.Value)
                        {
                            item.SignedDate = reader.GetDateTime(reader.GetOrdinal("pickdate"));

                            if (reader["picktime"] != (object)DBNull.Value)
                            {
                                string tmpTime = reader["picktime"].ToString().Trim();
                                string[] arTime = tmpTime.Split(":");
                                item.SignedDate = item.SignedDate.AddHours(Convert.ToInt32(arTime[0]));
                                item.SignedDate = item.SignedDate.AddMinutes(Convert.ToInt32(arTime[1]));
                            }
                        }

                        if (reader["PCShipped"] != (object)DBNull.Value)
                            item.PiecesShipped = Convert.ToInt32(reader["PCShipped"].ToString());

                        if (item.WeightTendered == item.WeightShipped && item.PiecesTendered == item.PiecesShipped && item.WeightTendered > 0 && item.PiecesTendered > 0)
                            item.Status = "C";
                        else
                            item.Status = "P";

                        if (reader["trndte"] != (object)DBNull.Value)
                            item.TansferDate = reader.GetDateTime(reader.GetOrdinal("trndte"));

                        if (reader["trnto"] != (object)DBNull.Value)
                            item.TansferTo = reader["trnto"].ToString().Trim();

                        if (reader["trneta"] != (object)DBNull.Value)
                            item.TansferETA = reader["trneta"].ToString().Trim();

                        list.Add(item);
                        ctr++;
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

        #region GetPODInformation
        public static List<PODItem> GetPODInformation(string awb)
        {
            List<PODItem> list = new List<PODItem>();
            PODItem item = new PODItem();
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT signby,pickdate,picktime,recpcs,trnwt,umeasur,custno,awbno,adduser,adddate,addtime,lckuser," +
                    "lckstat,lckdate,lcktime,tranlineno,emailpu " +
                    "FROM SOPODD01 " +
                    "WHERE ALLTRIM(awbno) = '" + awb + "' ";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new PODItem();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            PropertyInfo prop = item.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);

                            if (prop != null)
                                prop.SetValue(item, reader.GetValue(i));
                        }

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

        public static List<PODItem> GetPODInformation(string custNum, DateTime minDate, DateTime maxDate)
        {
            List<PODItem> list = new List<PODItem>();
            PODItem item = new PODItem();
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string minDateString = string.Format("datetime({0},{1},{2})", minDate.Year, minDate.Month, minDate.Day);
                string maxDateString = string.Format("datetime({0},{1},{2})", maxDate.Year, maxDate.Month, maxDate.Day);

                string query = "SELECT signby,pickdate,picktime,recpcs,trnwt,umeasur,custno,awbno,adduser,adddate,addtime,lckuser," +
                    "lckstat,lckdate,lcktime,tranlineno,emailpu " +
                    "FROM SOPODD01 " +
                    "WHERE pickdate >= " + minDateString + " " +
                    "AND pickdate <= " + maxDateString + " " +
                    "AND ALLTRIM(custno) = '" + custNum + "'";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new PODItem();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            PropertyInfo prop = item.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);

                            if (prop != null)
                                prop.SetValue(item, reader.GetValue(i));
                        }

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
