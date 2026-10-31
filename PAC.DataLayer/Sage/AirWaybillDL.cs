using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PAC.Common.Sage;
using PAC.Common.Tracking;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;

namespace PAC.DataLayer.Sage
{
    public class AirWaybillDL : BaseDL
    {
        #region GetRecentAirbills
        public static List<AirWaybillItem> GetRecentAirbills(string custNum, int count)
        {
            List<AirWaybillItem> list = new List<AirWaybillItem>();
            AirWaybillItem item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT TOP " + count + " s.sono, s.awbprefix, s.ordate, s.origin, s.dest, s.serv_level, s.poofkgs, s.pieces, s.weight, s.commodity, s.carrier, s.dimweight, " +
                    "s.wtshipped, s.pcshipped, m.fltNo, m.fltdate, SUM(m.pieces) as mantotal, SUM(m.weight) as manweight, p.signby, p.pickdate, p.picktime " +
                    "FROM somast01 s " +
                    "LEFT JOIN SOPODD01 p ON VAL(p.awbno) = VAL(s.sono) " +
                    "LEFT JOIN somanf01 m on val(m.sono) = val(s.sono) " +
                    "WHERE ALLTRIM(s.custno) == '" + custNum + "' " +
                    "GROUP BY s.sono, s.awbprefix, s.ordate, s.origin, s.dest, s.serv_level, s.poofkgs, s.pieces, s.weight, s.commodity, s.carrier, s.dimweight, s.wtshipped, s.pcshipped, m.fltNo, m.fltdate, p.signby, p.pickdate, p.picktime " +
                    "ORDER BY ordate desc";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new AirWaybillItem();

                        if (reader["sono"] != (object)DBNull.Value)
                            item.Sono = reader["sono"].ToString().Trim();

                        if (reader["awbprefix"] != (object)DBNull.Value)
                            item.AWBPrefix = reader["awbprefix"].ToString().Trim();

                        if (reader["ordate"] != (object)DBNull.Value)
                            item.OrderDate = reader.GetDateTime(reader.GetOrdinal("ordate"));

                        if (reader["origin"] != (object)DBNull.Value)
                            item.Origin = reader["origin"].ToString().Trim();

                        if (reader["dest"] != (object)DBNull.Value)
                            item.Destination = reader["dest"].ToString().Trim();

                        if (reader["serv_level"] != (object)DBNull.Value)
                            item.ServiceLevel = reader["serv_level"].ToString().Trim();

                        if (reader["poofkgs"] != (object)DBNull.Value)
                            item.WeightUnit = reader["poofkgs"].ToString().Trim();

                        if (reader["pieces"] != (object)DBNull.Value)
                            item.PiecesTendered = reader.GetDecimal(reader.GetOrdinal("pieces"));

                        if (reader["weight"] != (object)DBNull.Value)
                            item.WeightTendered = reader.GetDecimal(reader.GetOrdinal("weight"));

                        if (reader["commodity"] != (object)DBNull.Value)
                            item.Commodity = reader["commodity"].ToString().Trim();

                        if (reader["carrier"] != (object)DBNull.Value)
                            item.Carrier = reader["carrier"].ToString().Trim();

                        if (reader["dimweight"] != (object)DBNull.Value)
                            item.DIMWeight = reader.GetDecimal(reader.GetOrdinal("dimweight"));

                        if (reader["wtshipped"] != (object)DBNull.Value)
                            item.WeightShipped = reader.GetDecimal(reader.GetOrdinal("wtshipped"));

                        if (reader["pcshipped"] != (object)DBNull.Value)
                            item.PiecesShipped = reader.GetDecimal(reader.GetOrdinal("pcshipped"));

                        if (reader["fltNo"] != (object)DBNull.Value)
                            item.FlightNumber = reader["fltNo"].ToString().Trim();

                        if (reader["fltdate"] != (object)DBNull.Value)
                            item.FlightDate = reader.GetDateTime(reader.GetOrdinal("fltdate"));

                        if (reader["mantotal"] != (object)DBNull.Value)
                            item.ManifestPieces = reader.GetDecimal(reader.GetOrdinal("mantotal"));

                        if (reader["manweight"] != (object)DBNull.Value)
                            item.ManifestWeight = reader.GetDecimal(reader.GetOrdinal("manweight"));

                        if (reader["SignBy"] != (object)DBNull.Value)
                            item.SignedBy = reader["SignBy"].ToString().Trim();

                        if (reader["pickdate"] != (object)DBNull.Value)
                        {
                            item.SignedDate = reader.GetDateTime(reader.GetOrdinal("pickdate"));

                            if (reader["picktime"] != (object)DBNull.Value)
                            {
                                string tmpTime = reader["picktime"].ToString().Trim();
                                string[] arTime = tmpTime.Split(":");
                                if (arTime.Length > 1)
                                {
                                    int tmpHour = 0;
                                    int tmpMin = 0;
                                    if (int.TryParse(arTime[0], out tmpHour) && int.TryParse(arTime[1], out tmpMin))
                                    {
                                        item.SignedDate = item.SignedDate.AddHours(tmpHour);
                                        item.SignedDate = item.SignedDate.AddMinutes(tmpMin);
                                    }
                                }
                            }
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

        #region GetOpenAirWaybills
        public static List<AirWaybillItem> GetOpenAirWaybills(string custNum)
        {
            List<AirWaybillItem> list = new List<AirWaybillItem>();
            AirWaybillItem item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT distinct s.sostat, s.sono, s.awbprefix, s.ordate, s.origin, s.dest, s.serv_level, s.poofkgs, s.pieces, s.weight, s.commodity, s.carrier, s.dimweight, " +
                    "s.wtshipped, s.pcshipped, m.fltdate " +
                    "FROM somast01 s " +
                    "LEFT JOIN somanf01 m on val(m.sono) = val(s.sono) " +
                    "WHERE ALLTRIM(s.custno) == '" + custNum + "' " +
                    "AND s.sostat = ' ' " +
                    "AND s.pcshipped < s.pieces " +
                    "AND (m.fltdate > {^2026-01-01} or ISNULL(m.fltdate)) " +
                    "ORDER BY ordate desc";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new AirWaybillItem();

                        if (reader["sono"] != (object)DBNull.Value)
                            item.Sono = reader["sono"].ToString().Trim();

                        if (reader["awbprefix"] != (object)DBNull.Value)
                            item.AWBPrefix = reader["awbprefix"].ToString().Trim();

                        if (reader["ordate"] != (object)DBNull.Value)
                            item.OrderDate = reader.GetDateTime(reader.GetOrdinal("ordate"));

                        if (reader["origin"] != (object)DBNull.Value)
                            item.Origin = reader["origin"].ToString().Trim();

                        if (reader["dest"] != (object)DBNull.Value)
                            item.Destination = reader["dest"].ToString().Trim();

                        if (reader["serv_level"] != (object)DBNull.Value)
                            item.ServiceLevel = reader["serv_level"].ToString().Trim();

                        if (reader["poofkgs"] != (object)DBNull.Value)
                            item.WeightUnit = reader["poofkgs"].ToString().Trim();

                        if (reader["pieces"] != (object)DBNull.Value)
                            item.PiecesTendered = reader.GetDecimal(reader.GetOrdinal("pieces"));

                        if (reader["weight"] != (object)DBNull.Value)
                            item.WeightTendered = reader.GetDecimal(reader.GetOrdinal("weight"));

                        if (reader["commodity"] != (object)DBNull.Value)
                            item.Commodity = reader["commodity"].ToString().Trim();

                        if (reader["carrier"] != (object)DBNull.Value)
                            item.Carrier = reader["carrier"].ToString().Trim();

                        if (reader["dimweight"] != (object)DBNull.Value)
                            item.DIMWeight = reader.GetDecimal(reader.GetOrdinal("dimweight"));

                        if (reader["wtshipped"] != (object)DBNull.Value)
                            item.WeightShipped = reader.GetDecimal(reader.GetOrdinal("wtshipped"));

                        if (reader["pcshipped"] != (object)DBNull.Value)
                            item.PiecesShipped = reader.GetDecimal(reader.GetOrdinal("pcshipped"));

                        if (reader["fltdate"] != (object)DBNull.Value)
                            item.FlightDate = reader.GetDateTime(reader.GetOrdinal("fltdate"));

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

        #region GetShipmentDetails
        public static List<ShipmentItem> GetShipmentDetails(string sono)
        {
            List<ShipmentItem> list = new List<ShipmentItem>();
            ShipmentItem item = null;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT s.sono, s.awbprefix, s.ordate, s.origin, s.dest, s.serv_level, s.poofkgs, s.pieces, s.weight, s.commodity, " +
                    "s.dimweight, s.wtshipped, s.pcshipped " +
                    "FROM somast01 s " +
                    "WHERE VAL(s.sono) = " + sono + " " +
                    "ORDER BY ordate desc";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        item = new ShipmentItem();

                        if (reader["sono"] != (object)DBNull.Value)
                            item.Sono = reader["sono"].ToString().Trim();

                        if (reader["awbprefix"] != (object)DBNull.Value)
                            item.AWBPrefix = reader["awbprefix"].ToString().Trim();

                        if (reader["ordate"] != (object)DBNull.Value)
                            item.OrderDate = reader.GetDateTime(reader.GetOrdinal("ordate"));

                        if (reader["origin"] != (object)DBNull.Value)
                            item.Origin = reader["origin"].ToString().Trim();

                        if (reader["dest"] != (object)DBNull.Value)
                            item.Destination = reader["dest"].ToString().Trim();

                        if (reader["serv_level"] != (object)DBNull.Value)
                            item.ServiceLevel = reader["serv_level"].ToString().Trim();

                        if (reader["poofkgs"] != (object)DBNull.Value)
                            item.WeightUnit = reader["poofkgs"].ToString().Trim();

                        if (reader["pieces"] != (object)DBNull.Value)
                            item.Pieces = reader.GetDecimal(reader.GetOrdinal("pieces"));

                        if (reader["weight"] != (object)DBNull.Value)
                            item.Weight = reader.GetDecimal(reader.GetOrdinal("weight"));

                        if (reader["commodity"] != (object)DBNull.Value)
                            item.Commodity = reader["commodity"].ToString().Trim();

                        if (reader["dimweight"] != (object)DBNull.Value)
                            item.DIMWeight = reader.GetDecimal(reader.GetOrdinal("dimweight"));

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
