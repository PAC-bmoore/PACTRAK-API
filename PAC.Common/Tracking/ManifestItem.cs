using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Tracking
{
    public class ManifestItem
    {
        public string Sono { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Destination { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string WeightUnit { get; set; } = string.Empty;
        public decimal Pieces { get; set; }
        public decimal Weight { get; set; }
        public string ULDNum { get; set; } = string.Empty;
        public string FlightNum { get; set; } = string.Empty;
        public DateTime FlightDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int FlightID { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
