namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Oracle;

public class OracleCustomerAccount
{
    public string? AccountName { get; set; }
    public uint? AccountNumber { get; set; }
    public ulong? CustomerAccountId { get; set; }
    public ulong? PartyId { get; set; }
    public string? OrigSystemReference { get; set; }
    public string? SalesforceId { get; set; }
    public string? OssId { get; set; }
    public string? AccountType { get; set; }
    public string? AccountSubType { get; set; }
    public List<OracleCustomerAccountContact>? Contacts { get; set; }
    public List<OracleCustomerAccountSite>? Sites { get; set; }
}
