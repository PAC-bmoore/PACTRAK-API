using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Sage
{
    public class InvoiceItem
    {
        public string Item { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0M;
        public decimal Quantity { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
    }
}
