using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Tracking
{
    public class ManifestItem
    {
        public string SONumber { get; set; } = string.Empty;
        public DateTime SODate { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string PoofKGS { get; set; } = string.Empty;
        public decimal Pieces { get; set; }
        public decimal Weight { get; set; }
        public string ULDNum { get; set; } = string.Empty;
        public string FlightNum { get; set; } = string.Empty;
        public DateTime FlightDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string FlightID { get; set; } = string.Empty;
    }
}
