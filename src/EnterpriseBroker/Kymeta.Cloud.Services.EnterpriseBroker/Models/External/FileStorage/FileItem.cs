namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.External.FileStorage
{
    public class FileItem
    {
        public string AbsoluteUri { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileHash { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string Uri { get; set; }
        public Guid? UploadedById { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}
