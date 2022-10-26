using Microsoft.AspNetCore.StaticFiles;

namespace AzureBlobAPI.Helpers
{
    public static class FileExtentions
    {
        private static readonly FileExtensionContentTypeProvider Provider =
            new FileExtensionContentTypeProvider();

        public static string GetContentType(this string fileName)
        {
            if (!Provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
