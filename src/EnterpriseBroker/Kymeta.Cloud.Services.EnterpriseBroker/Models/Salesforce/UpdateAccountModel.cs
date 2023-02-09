namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce
{
    public class UpdateAccountModel : SalesforceActionObject
    {
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public string? OssAccountId { get; set; }
        public string? SubType { get; set; } // Used in SOAP request
        public string? TaxId { get; set; } // Used in SOAP request
        public string? AccountType { get; set; } // Used in SOAP request
        public string? OracleCustomerAccountId { get; set; } = null;
        public string? OracleCustomerProfileId { get; set; } = null;
        public string? OracleOrganizationId { get; set; } = null;
    }
}
