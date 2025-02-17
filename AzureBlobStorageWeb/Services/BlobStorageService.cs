using Azure.Storage.Blobs;

namespace AzureBlobStorageWeb.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = configuration["AzureStorage:ContainerName"]; // Read from configuration
        }
        // Upload file
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);
            await blobClient.UploadAsync(fileStream, true);

            return blobClient.Uri.ToString();
        }
        // Download file
        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(fileName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }
        // Delete file
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(fileName);
            return await blobClient.DeleteIfExistsAsync();
        }
        // List all files
        public async Task<List<string>> ListFilesAsync()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            List<string> files = new();

            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                files.Add(blobItem.Name);
            }

            return files;
        }
    }
}
