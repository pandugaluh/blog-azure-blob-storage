using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobAPI.Helpers;
using AzureBlobAPI.Models;
using AzureBlobAPI.Settings;
using Microsoft.Extensions.Options;

namespace AzureBlobAPI.Services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly string _connectionString, _containerName, _baseFolder;
        private readonly BlobContainerClient _container;

        public BlobStorage(IOptions<BlobSettings> options)
        {
            var blobSettings = options.Value;
            _connectionString = blobSettings.ConnectionString;
            _containerName = blobSettings.Container;
            _baseFolder = blobSettings.BaseFolde;
            _container = new BlobContainerClient(_connectionString, _containerName);
            _container.CreateIfNotExists();
        }

        public async Task<BlobDetail> GetBlobAsync(string fileName)
        {
            var blobPath = Path.Combine(_baseFolder, fileName).Replace("\\", "/");
            var blobClient = _container.GetBlobClient(blobPath);
            var blobDownloadInfo = await blobClient.DownloadAsync();
            return new BlobDetail
            {
                Content = blobDownloadInfo.Value.Content,
                ContentType = blobDownloadInfo.Value.Details.ContentType
            };
        }

        public async Task<List<string>> GetBlobListAsync()
        {
            string prefix = $"{_baseFolder}/";

            var items = new List<string>();

            await foreach (var blobItem in _container.GetBlobsAsync())
            {
                if (blobItem.Name.StartsWith(prefix))
                {
                    items.Add(blobItem.Name.Replace(prefix, ""));
                }
            }

            return items;
        }

        public async Task UploadBlobAsync(string filePath, string fileName)
        {
            var blobPath = Path.Combine(_baseFolder, fileName).Replace("\\", "/");
            var blobClient = _container.GetBlobClient(blobPath);
            await blobClient.UploadAsync(filePath, new BlobHttpHeaders { ContentType = filePath.GetContentType() });
        }

        public async Task DeleteBlobAsync(string fileName)
        {
            var blobPath = Path.Combine(_baseFolder, fileName).Replace("\\", "/");
            var blobClient = _container.GetBlobClient(blobPath);
            await blobClient.DeleteIfExistsAsync();
        }
        
    }
}
