namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceReportViewModel
    {
        public string? RecordId { get; set; }
        public string? ProductCode      { get; set; }
        public string? Stage            { get; set; }
        public string? ProductName      { get; set; }
        public string? ProductGen       { get; set; }
        public string? ProductType      { get; set; }
        public string? ProductFamily        { get; set; }
        public string? TerminalCategory { get; set; }
        public string? PriceBookName        { get; set; }
        public string? ListPrice        { get; set; }
        public string? ItemDetail       { get; set; }
        public string? ProductDesc      { get; set; }
        public bool? Unavailable { get; set; }
        public string? TargetMarkets { get; set; }
        public string? ProductKit { get; set; }
    }

    public class SalesforceReportListPriceViewModel
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
    }
}
