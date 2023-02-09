using Kymeta.Cloud.Services.EnterpriseBroker.Models.External.FileStorage;
using System.Text.Json;

namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients
{
    public interface IFileStorageClient
    {
        Task<bool> UploadBlobFile(Stream fileContent, string fileName, string storageAccount, string container, string path);
        Task<IEnumerable<FileItem>> ListBlobs(string accountKeyIdentifier, string storageAccount, string container, string? path = null);
        Task<bool> DeleteBlob(string accountKeyIdentifier, string storageAccount, string container, string path);
    }
    public class FileStorageClient : IFileStorageClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly ILogger<FileStorageClient> _logger;

        public FileStorageClient(HttpClient client, IConfiguration config, ILogger<FileStorageClient> logger)
        {
            client.BaseAddress = new Uri(config["Api:FileStorage"]);
            client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

            _client = client;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Upload files to Azure Blob Storage
        /// </summary>
        /// <param name="fileContent">File content to be uploaded to Azure Blob Storage.</param>
        /// <param name="fileName">Name of the file to be uploaded.</param>
        /// <param name="storageAccount">Name of the target Storage Account in Azure Storage</param>
        /// <param name="container">Name of the container within the Storage Account.</param>
        /// <param name="path">Specify a directory path within the container.</param>
        /// <returns></returns>
        public async Task<bool> UploadBlobFile(Stream fileContent, string fileName, string storageAccount, string container, string path)
        {
            // convert stream to byte array
            using var memoryStream = new MemoryStream();
            fileContent.CopyTo(memoryStream);
            var fileContentBytes = memoryStream.ToArray();

            // init the form data post body
            MultipartFormDataContent form = new()
            {
                { new StringContent(storageAccount), "storageaccount" },
                { new StringContent(container), "containername" },
                { new StringContent(path), "path" },
                { new ByteArrayContent(fileContentBytes, 0, fileContentBytes.Length), "files",  fileName }
            };

            // send the file to FileStorage service to upload to blob storage (CDN)
            var response = await _client.PostAsync("/v1/blobs", form);

            // return upload result
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Fetch a list of Blob files from Azure Storage
        /// </summary>
        /// <param name="storageAccount">The Azure Storage Account name.</param>
        /// <param name="container">The name of the Container within the Storage Account containing the blobs.</param>
        /// <param name="path">[Optional] Specify a path to narrow the results within the container.</param>
        /// <returns>A list of blob files</returns>
        public async Task<IEnumerable<FileItem>> ListBlobs(string accountKeyIdentifier, string storageAccount, string container, string? path = null)
        {
            // init payload
            var payload = new FileStorageRequest 
            {
                AccountIdentifier = accountKeyIdentifier,
                StorageAccount = storageAccount,
                Container = container,
                Path = path
            };
            
            // fetch list of blobs from the specified storage account & container
            var response = await _client.PostAsJsonAsync("/v2/blobs/list", payload);
            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // TODO: throw error here?
                return null;
            }

            var blobs = JsonSerializer.Deserialize<IEnumerable<FileItem>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            // return upload result
            return blobs;
        }

        /// <summary>
        /// Delete a blob from Azure Storage
        /// </summary>
        /// <param name="accountKeyIdentifier">Identifier used to specify the config Key to use when accessing Azure Storage</param>
        /// <param name="storageAccount">Azure Storage Account name where the blob is hosted</param>
        /// <param name="container">The container the blob is associated with</param>
        /// <param name="path">Full path of the blob to delete.</param>
        /// <returns></returns>
        public async Task<bool> DeleteBlob(string accountKeyIdentifier, string storageAccount, string container, string path)
        {
            var payload = new FileStorageRequest
            {
                AccountIdentifier = accountKeyIdentifier,
                StorageAccount = storageAccount,
                Container = container,
                Path = path
            };
            // fetch list of blobs from the specified storage account & container
            var response = await _client.PostAsJsonAsync("/v2/blobs/delete", payload);

            // return upload result
            return response.IsSuccessStatusCode;
        }
    }
}
