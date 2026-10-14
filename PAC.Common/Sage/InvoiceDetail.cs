using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.Common.Sage
{
    public class InvoiceDetail
    {
        public string AWB { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int TotalPiecesShipped { get; set; }
        public decimal ActualWeight { get; set; }
        public string UnitOfMeasure { get; set; } = string.Empty;
        public decimal ChargeableWeight { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal InvoiceSubTotal { get; set; }
        public decimal InvoiceTax { get; set; }
        public decimal InvoiceTotal { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public string Customer { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string SalesOrderNumber { get; set; } = string.Empty;
        public DateTime TansferDate { get; set; }
        public string TansferTo { get; set; } = string.Empty;
        public string TansferETA { get; set; } = string.Empty;
        public List<PODItem> POD { get; set; } = new List<PODItem>();
        public string ServiceLevel { get; set; } = string.Empty;
        public string FreightType { get; set; } = string.Empty;
        public string PaymentTerms { get; set; } = string.Empty;
        public string SpotNumber { get; set; } = string.Empty;
    }
}
