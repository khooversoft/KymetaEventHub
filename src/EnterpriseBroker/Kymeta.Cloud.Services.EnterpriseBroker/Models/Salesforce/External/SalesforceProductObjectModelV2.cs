using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External;

public class SalesforceProductObjectModelV2
{
    public string Id { get; set; }
    public string? SalesforceId { get; set; }
    public string? Name { get; set; }
    public float? WholesalePrice { get; set; }
    public float? MsrpPrice { get; set; }
    public float? DiscountTier2Price { get; set; }
    public float? DiscountTier3Price { get; set; }
    public float? DiscountTier4Price { get; set; }
    public float? DiscountTier5Price { get; set; }
    public bool? Mil { get; set; }
    public bool? Comm { get; set; }
    [JsonProperty("cpqProductDescription__c")]
    public string? Description { get; set; }
    public bool? Unavailable { get; set; } = false;
    public string? ProductType { get; set; }
    public string? ProductSubType { get; set; }
    public string? ProductFamily { get; set; }
    public int? Score { get; set; } = 0; // Connectivity Only
    public IEnumerable<string>? Assets { get; set; } // Terminal, Accessory Only
}

public class SalesforceProductDiscountObjectModel
{
    public string Id { get; set; }
    public string? SalesforceId { get; set; }
    public double? Tier1Percent { get; set; }
    public double? Tier2Percent { get; set; }
    public double? Tier3Percent { get; set; }
    public double? Tier4Percent { get; set; }
    public double? Tier5Percent { get; set; }
}
