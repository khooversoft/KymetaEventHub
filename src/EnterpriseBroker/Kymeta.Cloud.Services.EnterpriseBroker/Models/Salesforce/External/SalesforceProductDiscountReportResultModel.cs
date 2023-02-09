using Newtonsoft.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceProductDiscountReportResultModel
    {
        public SalesforceReportFactmap factMap { get; set; }
        public SalesforceReportMetadata reportMetadata { get; set; }
    }

    public class SalesforceReportFactmap
    {
        [JsonProperty(PropertyName = "T!T")]
        public ProductReportResult ReportResult { get; set; }
    }

    public class ProductReportResult
    {
        public SalesforceReportRowModel[] rows { get; set; }
    }

    public class SalesforceReportRowModel
    {
        public SalesforceReportDatacell[] dataCells { get; set; }
    }

    public class SalesforceReportDatacell
    {
        public object value { get; set; }
    }

    public class SalesforceReportMetadata
    {
        public string[] detailColumns { get; set; }
    }
}
