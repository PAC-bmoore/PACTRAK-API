using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PAC.Common.Sage
{
    public class Customer
    {
        [JsonPropertyName("CustNum")]
        public string custno { get; set; } = string.Empty;
        [JsonPropertyName("CompanyName")]
        public string company { get; set; } = string.Empty;
        [JsonPropertyName("ContactName")]
        public string contact { get; set; } = string.Empty;
        [JsonPropertyName("Email")]
        public string email { get; set; } = string.Empty;
        [JsonPropertyName("ContactTitle")]
        public string title { get; set; } = string.Empty;
        [JsonPropertyName("Address1")]
        public string address1 { get; set; } = string.Empty;
        [JsonPropertyName("Address2")]
        public string address2 { get; set; } = string.Empty;
        [JsonPropertyName("City")]
        public string city { get; set; } = string.Empty;
        [JsonPropertyName("State")]
        public string addrstate { get; set; } = string.Empty;
        [JsonPropertyName("Zip")]
        public string zip { get; set; } = string.Empty;
        [JsonPropertyName("Country")]
        public string country { get; set; } = string.Empty;
        [JsonPropertyName("Phone")]
        public string phone { get; set; } = string.Empty;
        [JsonPropertyName("Curreny")]
        public string currency { get; set; } = string.Empty;
        [JsonPropertyName("Fax")]
        public string faxno { get; set; } = string.Empty;
        [JsonIgnore]
        public string webpwd { get; set; } = string.Empty;
        [JsonPropertyName("ID")]
        public int id_col { get; set; }
        [JsonPropertyName("UserName")]
        public string username { get; set; } = string.Empty;
        [JsonIgnore]
        public string webpwdhash { get; set; } = string.Empty;
        [JsonPropertyName("CustomerStatus")]
        public string custstat {  get; set; }  = string.Empty;
    }
}
