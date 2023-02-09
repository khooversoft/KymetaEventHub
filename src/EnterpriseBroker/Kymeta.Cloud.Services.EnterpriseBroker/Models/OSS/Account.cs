using System.Text.Json.Serialization;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public class Account
{
    public Guid? Id { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? CreatedById { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public Guid? ModifiedById { get; set; }
    public string? Name { get; set; }
    public Guid? ParentId { get; set; }

    public string? SalesforceAccountId { get; set; }
    public string? OracleAccountId { get; set; }

    public bool? MilitaryPriceBook { get; set; }
    public bool? CommercialPriceBook { get; set; }
    public int? DiscountTier { get; set; } = 1;
    public decimal? DiscountPercentOverride { get; set; }
    public decimal? DiscountFlatOverride { get; set; }
    public bool? WholesalePricesVisible { get; set; }
    public bool? MsrpPricesVisible { get; set; }
    public bool? ConfiguratorVisible { get; set; }

    public CreatedOriginEnum? Origin { get; set; }
    public bool? Enabled { get; set; }

    // Optional
    public string? ParentName { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CreatedOriginEnum
{
    SF,
    OSS
}