using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Sage
{
    public class AirWaybillItem
    {
        public string Sono { get; set; } = string.Empty;
        public string AWBPrefix { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string ServiceLevel { get; set; } = string.Empty;
        public string WeightUnit { get; set; } = string.Empty;
        public decimal PiecesTendered { get; set; }
        public decimal WeightTendered { get; set; }
        public decimal DIMWeight { get; set; }
        public string Commodity { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        public decimal PiecesShipped { get; set; }
        public decimal WeightShipped { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public DateTime FlightDate { get; set; }
        public decimal ManifestPieces { get; set; }
        public decimal ManifestWeight { get; set; }
        public string SignedBy { get; set; } = string.Empty;
        public DateTime SignedDate { get; set; }
    }
}
