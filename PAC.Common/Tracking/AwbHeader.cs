using System.Text.Json.Serialization;

namespace PAC.Common.Tracking
{
    /// <summary>
    /// One sales order (air waybill) as the portal reads it: the SOMAST01
    /// columns the portal's tracking list and detail need, with the field
    /// names the portal client already uses.
    /// </summary>
    public class AwbHeader
    {
        [JsonPropertyName("sono")] public string Sono { get; set; } = string.Empty;
        [JsonPropertyName("awbprefix")] public string AwbPrefix { get; set; } = string.Empty;
        [JsonPropertyName("custno")] public string CustNo { get; set; } = string.Empty;
        [JsonPropertyName("company")] public string Company { get; set; } = string.Empty;
        [JsonPropertyName("sodate")] public DateTime? SoDate { get; set; }
        [JsonPropertyName("ordate")] public DateTime? OrDate { get; set; }
        [JsonPropertyName("sostat")] public string SoStat { get; set; } = string.Empty;
        [JsonPropertyName("dest")] public string Dest { get; set; } = string.Empty;
        [JsonPropertyName("origin")] public string Origin { get; set; } = string.Empty;
        [JsonPropertyName("serv_level")] public string ServLevel { get; set; } = string.Empty;
        [JsonPropertyName("poofkgs")] public string WeightUnit { get; set; } = string.Empty;
        [JsonPropertyName("weight")] public decimal Weight { get; set; }
        [JsonPropertyName("pieces")] public int Pieces { get; set; }
        [JsonPropertyName("wtshipped")] public decimal WeightShipped { get; set; }
        [JsonPropertyName("pcshipped")] public int PiecesShipped { get; set; }
        [JsonPropertyName("commodity")] public string Commodity { get; set; } = string.Empty;
        [JsonPropertyName("ftype")] public string FreightType { get; set; } = string.Empty;
        [JsonPropertyName("dimweight")] public decimal DimWeight { get; set; }
        [JsonPropertyName("cargostatus")] public string CargoStatus { get; set; } = string.Empty;
    }
}
