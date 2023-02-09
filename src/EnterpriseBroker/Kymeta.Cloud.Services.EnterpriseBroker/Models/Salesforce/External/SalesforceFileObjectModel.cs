namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.Salesforce.External
{
    public class SalesforceFileObjectModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? VersionNumber { get; set; }
        public string? MimeType { get; set; }
        public string? FileExtension { get; set; }
        public string? DownloadUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
    }
}
