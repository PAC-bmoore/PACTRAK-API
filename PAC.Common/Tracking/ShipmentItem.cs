using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PAC.Common.Tracking
{
    public class ShipmentItem
    {
        public string Sono { get; set; } = string.Empty;
        public decimal Pieces { get; set; }
        public decimal Weight { get; set; }
        public decimal DIMWeight { get; set; }
        public decimal ChargeableWeight 
        {
            get 
            { 
                decimal tmp = DIMWeight > Weight ? DIMWeight : Weight; 
                return tmp; 
            } 
        }
        public string Destination { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string WeightUnit { get; set; } = string.Empty;
        public string ServiceLevel { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Commodity { get; set; } = string.Empty;
        public string AWBPrefix { get; set; } = string.Empty;

    }
}
