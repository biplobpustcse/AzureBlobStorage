using AzureBlobStorageWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorageWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly BlobStorageService _blobService;

        public BlobController(BlobStorageService blobService)
        {
            //_blobService = blobService;
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
        }

        // Upload file
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using var stream = file.OpenReadStream();
            var fileUrl = await _blobService.UploadFileAsync(stream, file.FileName);

            return Ok(new { FileUrl = fileUrl });
        }

        // Download file
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var fileStream = await _blobService.DownloadFileAsync(fileName);
            if (fileStream == null)
                return NotFound("File not found");

            return File(fileStream, "application/octet-stream", fileName);
        }
        // Delete file
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            var result = await _blobService.DeleteFileAsync(fileName);
            return result ? Ok("File deleted successfully") : NotFound("File not found");
        }
        // List all files
        [HttpGet("list")]
        public async Task<IActionResult> ListFiles()
        {
            var files = await _blobService.ListFilesAsync();
            return Ok(files);
        }
    }
}
