namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class UpdateOracleOrganizationModel
{
    public ulong PartyId { get; set; }
    public ulong PartyNumber { get; set; }
    public string OrganizationName { get; set; }
    public string Type { get; set; }
    public string SourceSystem { get; set; }
    public string SourceSystemReferenceValue { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
}

