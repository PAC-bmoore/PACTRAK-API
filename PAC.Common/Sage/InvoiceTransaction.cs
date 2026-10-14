using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PAC.Common.Sage
{
    public class InvoiceTransaction
    {
        [JsonPropertyName("InvoiceNo")]
        public string invno { get; set; } = string.Empty;
        [JsonPropertyName("Item")]
        public string item { get; set; } = string.Empty;
        [JsonPropertyName("CustNo")]
        public string custno { get; set; } = string.Empty;
        [JsonPropertyName("Description")]
        public string descrip { get; set; } = string.Empty;
        [JsonPropertyName("UOM")]
        public string umeasur { get; set; } = string.Empty;
        [JsonPropertyName("Price")]
        public decimal price { get; set; }
        [JsonPropertyName("QtyOrdered")]
        public decimal qtyord { get; set; }
        [JsonPropertyName("QtyShipped")]
        public decimal qtyshp { get; set; }
        [JsonPropertyName("InvoiceDate")]
        public DateTime invdte { get; set; }
        [JsonPropertyName("PONum")]
        public string ponum { get; set; } = string.Empty;
        [JsonPropertyName("ExtPrice")]
        public decimal extprice { get; set; }
        [JsonPropertyName("TransSeq")]
        public string transeq { get; set; } = string.Empty;
        [JsonPropertyName("TransLineNo")]
        public decimal tranlineno { get; set; }
        [JsonPropertyName("AWBNo")]
        public string sono { get; set; } = string.Empty;
    }
}
