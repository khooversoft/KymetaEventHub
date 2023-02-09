namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceFileResponseModel
    {
        public bool HasErrors { get; set; }
        public List<SalesforceFileResultModel> Results { get; set; }
    }
}
