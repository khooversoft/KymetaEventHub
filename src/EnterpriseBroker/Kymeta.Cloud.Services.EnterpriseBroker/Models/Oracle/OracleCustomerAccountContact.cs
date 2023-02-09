using static Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle.SOAP.OracleSoapTemplates;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class OracleCustomerAccountContact
{
    public string? OrigSystemReference { get; set; }
    public ulong? ContactPersonId { get; set; }
    public ulong? RelationshipId { get; set; }
    public List<string>? ResponsibilityTypes { get; set; }
    public bool? IsPrimary { get; set; }
    public ulong? CustomerAccountRoleId { get; set; }
}
