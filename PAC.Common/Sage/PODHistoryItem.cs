using PAC.Common.Tracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Sage
{
    public class PODHistoryItem
    {
        public DateTime DateTendered { get; set; }
        public DateTime FlightDate { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int PiecesTendered { get; set; }
        public string AWBPrefix { get; set; } = string.Empty;
        public string AWBNumber { get; set; } = string.Empty;
        public decimal WeightTendered { get; set; }
        public decimal WeightShipped { get; set; }
        public string WeightUnit { get; set; } = string.Empty;
        public int PiecesPickedUp { get; set; }
        public int PiecesShipped { get; set; }
        public string SignedBy { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime SignedDate { get; set; }
        public DateTime TansferDate { get; set; }
        public string TansferTo { get; set; } = string.Empty;
        public string TansferETA { get; set; } = string.Empty;
        public string Carrier { get; set; } = string.Empty;
        public decimal TransferCost { get; set; }
        public DateTime DropOffDate { get; set; }
        public string SOStat { get; set; } = string.Empty;
        public string SOType { get; set; } = string.Empty;
        public decimal DIMWeight { get; set; }
    }
}
