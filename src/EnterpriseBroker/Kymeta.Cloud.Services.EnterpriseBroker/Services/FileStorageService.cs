using Kymeta.Cloud.Services.EnterpriseBroker.Models.External.FileStorage;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Services
{
    public interface IFileStorageService
    {
        Task<IEnumerable<FileItem>> GetBlobs(string accountKeyIdentifier, string storageAccountName, string container, string? path = null);
        Task<bool> DeleteBlobs(string accountKeyIdentifier, string storageAccountName, string container, IEnumerable<string> blobsToDelete);
    }

    public class FileStorageService : IFileStorageService
    {
        private readonly IFileStorageClient _fileStorageClient;

        public FileStorageService(IFileStorageClient fileStorageClient)
        {
            _fileStorageClient = fileStorageClient;
        }

        public async Task<IEnumerable<FileItem>> GetBlobs(string accountKeyIdentifier, string storageAccountName, string container, string? path = null)
        {
            // validate we have the necessary values from arguments/config
            if (string.IsNullOrEmpty(accountKeyIdentifier)) throw new ArgumentNullException(nameof(accountKeyIdentifier));
            if (string.IsNullOrEmpty(storageAccountName)) throw new ArgumentNullException(nameof(storageAccountName));
            if (string.IsNullOrEmpty(container)) throw new ArgumentNullException(nameof(container));

            var blobsResult = await _fileStorageClient.ListBlobs(accountKeyIdentifier, storageAccountName, container, path);
            return blobsResult;
        }

        public async Task<bool> DeleteBlobs(string accountKeyIdentifier, string storageAccountName, string container, IEnumerable<string> blobPaths)
        {
            // validate we have the necessary values from arguments/config
            if (string.IsNullOrEmpty(accountKeyIdentifier)) throw new ArgumentNullException(nameof(accountKeyIdentifier));
            if (string.IsNullOrEmpty(storageAccountName)) throw new ArgumentNullException(nameof(storageAccountName));
            if (string.IsNullOrEmpty(container)) throw new ArgumentNullException(nameof(container));

            // build list of deletion tasks to delete blobs from Azure Storage
            var deleteBlobTasks = new List<Task<bool>>();
            foreach (var path in blobPaths)
            {
                deleteBlobTasks.Add(_fileStorageClient.DeleteBlob(accountKeyIdentifier, storageAccountName, container, path));
            }

            // process all deletion tasks
            await Task.WhenAll(deleteBlobTasks);

            // get the results
            var deletionResults = deleteBlobTasks.Select(t => t.Result).ToList();

            // return true or false based on cumulative results of deletions
            return !deletionResults.Any(isSuccess => !isSuccess);
        }
    }
}
