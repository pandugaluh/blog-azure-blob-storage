using AzureBlobAPI.Models;

namespace AzureBlobAPI.Services
{
    public interface IBlobStorage
    {
        Task<BlobDetail> GetBlobAsync(string fileName);
        Task<List<string>> GetBlobListAsync();
        Task UploadBlobAsync(string filePath, string fileName);
        Task DeleteBlobAsync(string fileName);
    }
}