namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Responses
{
    public class AccountResponse : SalesforceProcessResponse
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
    }

    public class AccountChildResponse
    {
        public string? OracleEntityType { get; set; }
        public string? OracleId { get; set; }
        public string? SalesforceId { get; set; }
    }
}
