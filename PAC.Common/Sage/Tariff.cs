using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Sage
{
    public class Tariff
    {
        public string CustNo { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public decimal BeginQty { get; set; }
        public decimal EndQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal BeginQty2 { get; set; }
        public decimal EndQty2 { get; set; }
        public decimal UnitPrice2 { get; set; }
        public string Curreny { get; set; } = string.Empty;
        public string ServiceLevel { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;

    }
}
