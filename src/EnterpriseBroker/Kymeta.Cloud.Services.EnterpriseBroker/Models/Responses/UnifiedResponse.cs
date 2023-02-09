namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses;

public class UnifiedResponse : SalesforceProcessResponse
{
    /// <summary>
    /// Id of the created/updated Oss Account
    /// </summary>
    public string? OssAccountId { get; set; }
    /// <summary>
    /// Id of the created/updated Oracle Customer Account
    /// </summary>
    public string? OracleCustomerAccountId { get; set; } = null;
    /// <summary>
    /// Id of the created/updated Oracle Customer Profile
    /// </summary>
    public string? OracleCustomerProfileId { get; set; } = null;
    /// <summary>
    /// Id of the created/updated Oracle Organization
    /// </summary>
    public string? OracleOrganizationId { get; set; } = null;

    // Optional when creating an account
    public IEnumerable<AccountChildResponse>? Contacts { get; set; }
    public IEnumerable<AccountChildResponse>? Addresses { get; set; }
    public IEnumerable<AccountV2>? ChildAccounts { get; set; }

    public string? OracleAddressId { get; set; }

    public string? OraclePersonId { get; set; }
}

