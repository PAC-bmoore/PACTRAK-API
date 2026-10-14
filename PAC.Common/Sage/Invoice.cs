using PAC.Common.Tracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Sage
{
    public class Invoice
    {
        public string Sono { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public decimal InvoiceSubTotal { get; set; }
        public decimal InvoiceTax { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public DateTime PaidDate { get; set; }
        public List<InvoiceItem> Items { get; set; }
        public string InvoiceStatus { get; set; } = string.Empty;
    }
}
