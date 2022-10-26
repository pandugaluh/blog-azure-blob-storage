using AzureBlobAPI.Models;
using AzureBlobAPI.Services;

namespace AzureBlobAPI.Presents
{
    public static class BlobPresents
    {
        public static void BlobEndpoint(this WebApplication app)
        {
            app.MapGet("/blob/{fileName}", GetBlob);
            app.MapGet("/blob", GetBlobList);
            app.MapPost("/blob", InsertBlob);
            app.MapDelete("/blob", DeleteBlob);
        }

        private static async Task<IResult> GetBlob(string fileName, IBlobStorage blobStorage)
        {
            try
            {
                var results = await blobStorage.GetBlobAsync(fileName);
                if (results == null) return Results.NotFound();
                return Results.File(results.Content, results.ContentType);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> GetBlobList(IBlobStorage blobStorage)
        {
            try
            {
                return Results.Ok(await blobStorage.GetBlobListAsync());
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> InsertBlob(BlobRequest request, IBlobStorage blobStorage)
        {
            try
            {
                await blobStorage.UploadBlobAsync(request.FilePath, request.FileName);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> DeleteBlob(string fileName, IBlobStorage blobStorage)
        {
            try
            {
                await blobStorage.DeleteBlobAsync(fileName);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}
