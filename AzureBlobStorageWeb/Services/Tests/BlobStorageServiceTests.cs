using Azure.Storage.Blobs;
using Moq;
using Xunit;
using Azure;
using AzureBlobStorageWeb.Services;
using Azure.Storage.Blobs.Models;

namespace AzureBlobStorageWeb.Tests.Services
{
    public class BlobStorageServiceTests
    {
        private readonly Mock<BlobServiceClient> _mockBlobServiceClient;
        private readonly Mock<BlobContainerClient> _mockBlobContainerClient;
        private readonly Mock<BlobClient> _mockBlobClient;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly BlobStorageService _blobStorageService;
        private readonly string _containerName = "test-container";

        public BlobStorageServiceTests()
        {
            _mockBlobServiceClient = new Mock<BlobServiceClient>();
            _mockBlobContainerClient = new Mock<BlobContainerClient>();
            _mockBlobClient = new Mock<BlobClient>();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c["AzureStorage:ContainerName"]).Returns(_containerName);

            // Setup the BlobServiceClient mock to return the mocked BlobContainerClient
            _mockBlobServiceClient.Setup(client => client.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(_mockBlobContainerClient.Object);

            // Setup the BlobContainerClient mock to return the mocked BlobClient
            _mockBlobContainerClient.Setup(client => client.GetBlobClient(It.IsAny<string>()))
                .Returns(_mockBlobClient.Object);

            _blobStorageService = new BlobStorageService(_mockBlobServiceClient.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task UploadFileAsync_ShouldReturnUri_WhenFileUploadedSuccessfully()
        {
            // Arrange
            var fileName = "test.txt";
            var fileStream = new MemoryStream(); // In a real test, this should contain data
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

            var expectedUri = "https://mockstorageaccount.blob.core.windows.net/test-container/" + uniqueFileName;

            // Mocking the Response for the UploadAsync method
            var mockResponse = new Mock<Response<BlobContentInfo>>();
            _mockBlobClient.Setup(client => client.UploadAsync(fileStream, true, CancellationToken.None))
                           .ReturnsAsync(mockResponse.Object); // Simulate successful upload response

            // Simulate the URI for the uploaded file
            _mockBlobClient.Setup(client => client.Uri)
                           .Returns(new Uri(expectedUri));

            // Act
            var result = await _blobStorageService.UploadFileAsync(fileStream, fileName);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(uniqueFileName, result);
            Assert.Equal(expectedUri, result); // Verify the returned URI
        }
        [Fact]
        public async Task DeleteFileAsync_ShouldReturnTrue_WhenFileDeletedSuccessfully()
        {
            // Arrange
            var mockResponse = new Mock<Response<bool>>();
            mockResponse.Setup(response => response.Value).Returns(true); // Simulate a successful deletion
            var fileName = "test.txt";
            _mockBlobClient.Setup(client => client.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, CancellationToken.None))
                           .ReturnsAsync(mockResponse.Object);

            // Act
            var result = await _blobStorageService.DeleteFileAsync(fileName);

            // Assert
            Assert.True(result);
        }
    }
}
