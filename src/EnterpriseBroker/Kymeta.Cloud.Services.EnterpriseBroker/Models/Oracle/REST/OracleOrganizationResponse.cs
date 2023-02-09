namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.REST;

public class OracleOrganizationResponse
{
    public ulong PartyId { get; set; }
    public string PartyNumber { get; set; }
    public string SourceSystem { get; set; }
    public string SourceSystemReferenceValue { get; set; }
    public string OrganizationName { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
    public string Type { get; set; }
}