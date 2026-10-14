using System.Data.OleDb;
using PAC.Common.Sage;
using PAC.Common.Tracking;

namespace PAC.DataLayer.Sage
{
    /// <summary>
    /// Tracking reads for the portal: sales order headers (SOMAST01),
    /// manifest lines (somanf01), proof of delivery (SOPODD01) and
    /// invoices (ARMAST01) joined to their orders. Every query takes its
    /// values as OLE DB parameters (the ? placeholders), never as text
    /// spliced into the statement.
    /// </summary>
    public class TrackingDL : BaseDL
    {
        private const string HeaderColumns =
            "So.SONO, So.AWBPREFIX, So.CUSTNO, C.COMPANY, So.SODATE, So.ORDATE, So.SOSTAT, So.DEST, So.ORIGIN, So.SERV_LEVEL, " +
            "So.POOFKGS, So.WEIGHT, So.PIECES, So.WTSHIPPED, So.PCSHIPPED, So.COMMODITY, So.FTYPE, So.DIMWEIGHT, So.CARGOSTATUS";

        /// <summary>Orders dated in the range, every customer or one.</summary>
        public static List<AwbHeader> GetHeadersByDateRange(DateTime minDate, DateTime maxDate, string? custNo)
        {
            var query = "SELECT " + HeaderColumns + " FROM SOMAST01 So LEFT JOIN arcust01 C ON C.CUSTNO = So.CUSTNO " +
                        "WHERE So.ORDATE >= ? AND So.ORDATE <= ? " +
                        (custNo is null ? "" : "AND ALLTRIM(So.CUSTNO) = ? ") +
                        "ORDER BY So.ORDATE DESC";
            var parameters = new List<object> { minDate.Date, maxDate.Date };
            if (custNo is not null) parameters.Add(custNo.Trim());
            return Read(query, parameters, ReadHeader);
        }

        /// <summary>A customer's orders that are not closed.</summary>
        public static List<AwbHeader> GetOpenHeaders(string custNo)
        {
            var query = "SELECT " + HeaderColumns + " FROM SOMAST01 So LEFT JOIN arcust01 C ON C.CUSTNO = So.CUSTNO " +
                        "WHERE ALLTRIM(So.CUSTNO) = ? AND So.SOSTAT <> 'C' ORDER BY So.ORDATE DESC";
            return Read(query, [custNo.Trim()], ReadHeader);
        }

        /// <summary>Manifest lines for one order. The column names beyond
        /// the four already used elsewhere (sono, fltdate, fltno, lnstat)
        /// follow the old PACTRAK service's output; confirm against
        /// somanf01 and rename here if they differ.</summary>
        public static List<ManifestItem> GetManifestItems(string sono)
        {
            var query = "SELECT m.sono, So.SODATE, So.DEST, So.ORIGIN, So.POOFKGS, m.pieces, m.weight, m.uldnum, m.fltno, m.fltdate, m.lnstat, m.fltid " +
                        "FROM somanf01 m LEFT JOIN SOMAST01 So ON So.SONO = m.sono " +
                        "WHERE ALLTRIM(m.sono) = ? AND (m.lnstat IS NULL OR m.lnstat <> 'X') ORDER BY m.fltdate";
            return Read(query, [sono.Trim()], r => new ManifestItem
            {
                SONumber = Text(r, "sono"),
                SODate = Date(r, "SODATE") ?? default,
                Destination = Text(r, "DEST"),
                Origin = Text(r, "ORIGIN"),
                PoofKGS = Text(r, "POOFKGS"),
                Pieces = Dec(r, "pieces"),
                Weight = Dec(r, "weight"),
                ULDNum = Text(r, "uldnum"),
                FlightNum = Text(r, "fltno"),
                FlightDate = Date(r, "fltdate") ?? default,
                Status = Text(r, "lnstat"),
                FlightID = Text(r, "fltid"),
            });
        }

        /// <summary>Proof of delivery for one order, one row per pickup.</summary>
        public static List<PODHistoryItem> GetPodsForAwb(string sono)
            => Read(PodQuery("WHERE ALLTRIM(So.SONO) = ? "), [sono.Trim()], ReadPod);

        /// <summary>Proof of delivery across every customer in the range.</summary>
        public static List<PODHistoryItem> GetPodsByDateRange(DateTime minDate, DateTime maxDate)
            => Read(PodQuery("WHERE So.ORDATE >= ? AND So.ORDATE <= ? AND ((So.SOSTAT = 'C') OR (So.WtShipped > 0 OR So.PCShipped > 0)) "),
                [minDate.Date, maxDate.Date], ReadPod);

        /// <summary>Invoices dated in the range across every customer,
        /// joined to their orders for the shipment columns.</summary>
        public static List<InvoiceDetail> GetInvoicesByDateRange(DateTime minDate, DateTime maxDate)
        {
            var query = "SELECT A.INVNO, A.INVDTE, A.ORDATE, A.INVAMT, A.TAX, A.CUSTNO, A.ORNUM, A.PTERMS, " +
                        "S.SONO, S.ORIGIN, S.DEST, S.PIECES, S.WEIGHT, S.DIMWEIGHT, S.POOFKGS, S.SERV_LEVEL, S.FTYPE, S.TRNDTE, S.TRNTO, S.TRNETA " +
                        "FROM ARMAST01 A LEFT JOIN SOMAST01 S ON S.SONO = A.ORNUM " +
                        "WHERE A.INVDTE >= ? AND A.INVDTE <= ? ORDER BY A.INVDTE DESC";
            return Read(query, [minDate.Date, maxDate.Date], r =>
            {
                var weight = Dec(r, "WEIGHT");
                var dim = Dec(r, "DIMWEIGHT");
                var total = Dec(r, "INVAMT");
                var tax = Dec(r, "TAX");
                return new InvoiceDetail
                {
                    AWB = Text(r, "INVNO"),
                    SalesOrderNumber = Text(r, "SONO"),
                    Customer = Text(r, "CUSTNO"),
                    Origin = Text(r, "ORIGIN"),
                    Destination = Text(r, "DEST"),
                    TotalPiecesShipped = (int)Dec(r, "PIECES"),
                    ActualWeight = weight,
                    ChargeableWeight = dim > weight ? dim : weight,
                    UnitOfMeasure = Text(r, "POOFKGS"),
                    InvoiceDate = Date(r, "INVDTE") ?? default,
                    OrderDate = Date(r, "ORDATE") ?? default,
                    InvoiceSubTotal = total - tax,
                    InvoiceTax = tax,
                    InvoiceTotal = total,
                    ServiceLevel = Text(r, "SERV_LEVEL"),
                    FreightType = Text(r, "FTYPE"),
                    PaymentTerms = Text(r, "PTERMS"),
                    TansferDate = Date(r, "TRNDTE") ?? default,
                    TansferTo = Text(r, "TRNTO"),
                    TansferETA = Text(r, "TRNETA"),
                };
            });
        }

        // ---- shared ---------------------------------------------------------

        private static string PodQuery(string where)
            => "SELECT DISTINCT So.SODATE, So.ORDATE, So.ORIGIN, So.DEST, So.PIECES, So.AWBPREFIX, So.SONO, So.WEIGHT, So.WTSHIPPED, So.POOFKGS, " +
               "PO.RECPCS, PO.SignBy, PO.pickdate, PO.picktime, So.PCShipped, So.trndte, So.trnto, So.trneta, So.SOSTAT, So.SOTYPE, So.DIMWEIGHT, m.fltdate, m.fltno " +
               "FROM SOMAST01 So LEFT JOIN SOPODD01 PO ON PO.AWBNO = So.SONO LEFT JOIN somanf01 m ON m.sono = So.sono " +
               where + "AND (m.lnstat IS NULL OR m.lnstat <> 'X') ORDER BY So.ORDATE DESC";

        private static AwbHeader ReadHeader(OleDbDataReader r) => new()
        {
            Sono = Text(r, "SONO"),
            AwbPrefix = Text(r, "AWBPREFIX"),
            CustNo = Text(r, "CUSTNO"),
            Company = Text(r, "COMPANY"),
            SoDate = Date(r, "SODATE"),
            OrDate = Date(r, "ORDATE"),
            SoStat = Text(r, "SOSTAT"),
            Dest = Text(r, "DEST"),
            Origin = Text(r, "ORIGIN"),
            ServLevel = Text(r, "SERV_LEVEL"),
            WeightUnit = Text(r, "POOFKGS"),
            Weight = Dec(r, "WEIGHT"),
            Pieces = (int)Dec(r, "PIECES"),
            WeightShipped = Dec(r, "WTSHIPPED"),
            PiecesShipped = (int)Dec(r, "PCSHIPPED"),
            Commodity = Text(r, "COMMODITY"),
            FreightType = Text(r, "FTYPE"),
            DimWeight = Dec(r, "DIMWEIGHT"),
            CargoStatus = Text(r, "CARGOSTATUS"),
        };

        private static PODHistoryItem ReadPod(OleDbDataReader r)
        {
            var item = new PODHistoryItem
            {
                DateTendered = Date(r, "SODATE") ?? default,
                FlightDate = Date(r, "fltdate") ?? default,
                FlightNumber = Text(r, "fltno"),
                Origin = Text(r, "ORIGIN"),
                Destination = Text(r, "DEST"),
                PiecesTendered = (int)Dec(r, "PIECES"),
                AWBPrefix = Text(r, "AWBPREFIX"),
                AWBNumber = Text(r, "SONO"),
                WeightTendered = Dec(r, "WEIGHT"),
                WeightShipped = Dec(r, "WTSHIPPED"),
                WeightUnit = Text(r, "POOFKGS"),
                PiecesPickedUp = (int)Dec(r, "RECPCS"),
                SignedBy = Text(r, "SignBy"),
                PickDate = Text(r, "pickdate"),
                PickTime = Text(r, "picktime"),
                PiecesShipped = (int)Dec(r, "PCShipped"),
                TansferDate = Date(r, "trndte") ?? default,
                TansferTo = Text(r, "trnto"),
                TansferETA = Text(r, "trneta"),
                SOStat = Text(r, "SOSTAT"),
                SOType = Text(r, "SOTYPE"),
                DIMWeight = Dec(r, "DIMWEIGHT"),
            };
            item.Status = item.WeightTendered == item.WeightShipped && item.PiecesTendered == item.PiecesShipped
                          && item.WeightTendered > 0 && item.PiecesTendered > 0 ? "C" : "P";
            return item;
        }

        private static List<T> Read<T>(string query, IReadOnlyList<object> parameters, Func<OleDbDataReader, T> map)
        {
            var list = new List<T>();
            using var conn = new OleDbConnection(SageConn);
            conn.Open();
            using var cmd = new OleDbCommand(query, conn);
            for (var i = 0; i < parameters.Count; i++) cmd.Parameters.AddWithValue("p" + i, parameters[i]);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(map(reader));
            return list;
        }

        private static string Text(OleDbDataReader r, string column)
        {
            var i = Ordinal(r, column);
            return i < 0 || r.IsDBNull(i) ? string.Empty : r.GetValue(i).ToString()!.Trim();
        }

        private static decimal Dec(OleDbDataReader r, string column)
        {
            var i = Ordinal(r, column);
            return i < 0 || r.IsDBNull(i) ? 0m : Convert.ToDecimal(r.GetValue(i));
        }

        private static DateTime? Date(OleDbDataReader r, string column)
        {
            var i = Ordinal(r, column);
            if (i < 0 || r.IsDBNull(i)) return null;
            var value = r.GetValue(i);
            if (value is DateTime d) return d == DateTime.MinValue ? null : d;
            return DateTime.TryParse(value.ToString(), out var parsed) ? parsed : null;
        }

        private static int Ordinal(OleDbDataReader r, string column)
        {
            try { return r.GetOrdinal(column); } catch (IndexOutOfRangeException) { return -1; }
        }
    }
}
