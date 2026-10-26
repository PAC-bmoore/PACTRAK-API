using PAC.Common.Sage;
using PAC.Common.Tracking;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;

namespace PAC.DataLayer.Sage
{
    public class ManifestDL : BaseDL
    {
        #region GetMinTariffs
        public static List<ManifestItem> GetManifestItems(string sono)
        {
            List<ManifestItem> list = new List<ManifestItem>();
            ManifestItem item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT m.sono, So.SODATE, So.DEST, So.ORIGIN, So.POOFKGS, m.pieces, m.weight, m.uldno, m.fltno, m.fltdate, m.fltid " +
                    "FROM somanf01 m " +
                    "LEFT JOIN SOMAST01 So ON VAL(So.SONO) = VAL(m.sono) " +
                    "WHERE VAL(m.sono) = " + sono + " " +
                    "AND (m.lnstat IS NULL OR m.lnstat <> 'X') " +
                    "ORDER BY m.fltdate";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new ManifestItem();

                        if (reader["sono"] != (object)DBNull.Value)
                            item.Sono = reader["sono"].ToString().Trim();

                        if (reader["SODATE"] != (object)DBNull.Value)
                            item.OrderDate = reader.GetDateTime(reader.GetOrdinal("SODATE"));

                        if (reader["origin"] != (object)DBNull.Value)
                            item.Origin = reader["origin"].ToString().Trim();

                        if (reader["dest"] != (object)DBNull.Value)
                            item.Destination = reader["dest"].ToString().Trim();

                        if (reader["poofkgs"] != (object)DBNull.Value)
                            item.WeightUnit = reader["poofkgs"].ToString().Trim();

                        if (reader["pieces"] != (object)DBNull.Value)
                            item.Pieces = reader.GetDecimal(reader.GetOrdinal("pieces"));

                        if (reader["weight"] != (object)DBNull.Value)
                            item.Weight = reader.GetDecimal(reader.GetOrdinal("weight"));

                        if (reader["fltno"] != (object)DBNull.Value)
                            item.FlightNum = reader["fltno"].ToString().Trim();

                        if (reader["uldno"] != (object)DBNull.Value)
                            item.ULDNum = reader["uldno"].ToString().Trim();

                        if (reader["fltdate"] != (object)DBNull.Value)
                            item.FlightDate = reader.GetDateTime(reader.GetOrdinal("fltdate"));

                        if (reader["fltid"] != (object)DBNull.Value)
                            item.FlightID = Convert.ToInt32(reader["fltno"].ToString().Trim()); 

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
