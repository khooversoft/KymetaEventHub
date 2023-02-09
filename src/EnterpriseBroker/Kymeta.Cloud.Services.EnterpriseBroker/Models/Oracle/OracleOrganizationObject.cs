namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class OracleOrganization
{
    public ulong PartyId { get; set; }
    public ulong PartyNumber { get; set; }
    public string OrganizationName { get; set; }
    public string Type { get; set; }
    public string OrigSystemReference { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
    public List<OraclePartySite>? PartySites { get; set; }
    public List<OracleOrganizationContact>? Contacts { get; set; }
}

public class OraclePartySite
{
    public ulong? PartyId { get; set; }
    public ulong? LocationId { get; set; }
    public ulong? PartySiteId { get; set; }
    public ulong? PartySiteNumber { get; set; }
    public string? PartySiteName { get; set; }
    public string OrigSystemReference { get; set; }
    public string? Comments { get; set; }
    public List<OraclePartySiteUse>? SiteUses { get; set; }
}

public class OraclePartySiteUse
{
    public ulong? PartySiteUseId { get; set; }
    public string? SiteUseType { get; set; }
}

public class OracleOrganizationContact
{
    public string OrigSystemReference { get; set; }
    public ulong ContactPartyId { get; set; }
    public ulong ContactPartyNumber { get; set; }
    public string PersonFirstName { get; set; }
    public string PersonLastName { get; set; }
}
