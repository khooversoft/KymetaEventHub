namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class CreateOracleOrganizationModel
{
    public string OrganizationName { get; set; }
    public string SourceSystem { get; set; }
    public string SourceSystemReferenceValue { get; set; }
    public string TaxpayerIdentificationNumber { get; set; }
}

