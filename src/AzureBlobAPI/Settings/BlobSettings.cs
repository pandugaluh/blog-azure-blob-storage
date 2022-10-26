namespace AzureBlobAPI.Settings
{
    public class BlobSettings
    {
        public const string BlobStotage = "BlobStotage";

        public string ConnectionString { get; set; }
        public string Container { get; set; }
        public string BaseFolde { get; set; }
    }
}
