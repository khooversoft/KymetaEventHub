namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.OSS;

public class AccountV2
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? OracleAccountId { get; set; }
    public string? SalesforceAccountId { get; set; }
    public bool? Enabled { get; set; }
    public CreatedOriginEnum? Origin { get; set; }
    public string? RelationshipType { get; set; } // Partner, End Customer, etc
    public string? AccountSubType { get; set; } // VAR, Distributor, etc
    public ParentAccount? Parent { get; set; }
    public PrimaryContact? PrimaryContact { get; set; }
    public ConfiguratorAccess? Configurator { get; set; }
}

public class ParentAccount
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}

public class PrimaryContact
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class ConfiguratorAccess
{
    public bool? MilitaryPriceBook { get; set; }
    public bool? CommercialPriceBook { get; set; }
    public int? DiscountTier { get; set; } = 1;
    public bool? WholesalePricesVisible { get; set; }
    public bool? MsrpPricesVisible { get; set; }
    public bool? ConfiguratorVisible { get; set; }
}
