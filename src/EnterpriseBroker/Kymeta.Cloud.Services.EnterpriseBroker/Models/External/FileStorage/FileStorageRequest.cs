namespace Kymeta.Cloud.Services.EnterpriseBroker.Models.External.FileStorage
{
    public class FileStorageRequest
    {
        public string? Path { get; set; }
        public string AccountIdentifier { get; set; } // string Id to use when fetching Account Key
        public string StorageAccount { get; set; }
        public string Container { get; set; }
        public string Disposition { get; set; }
    }
}
