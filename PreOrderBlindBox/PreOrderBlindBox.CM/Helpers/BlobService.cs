using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurcusProject.CM.Helpers
{
    public class BlobOject
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public Stream? Content { get; set; }
    }

    public class BlobService : IBlobService
    {
        private readonly IConfiguration _config;

        public BlobService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string?> UploadFile(IFormFile file)
        {
            try
            {
                var containerName = _config["BlobService:Container"];
                var connectString = _config["BlobService:ConnectionString"];

                if (file.Length > 0)
                {
                    var allowedFileTypes = new[]
                    {
                        "image/jpeg", "image/png", "image/gif",        // Hình ảnh
                        "application/pdf", "application/msword",       // Tài liệu văn bản
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "application/vnd.ms-powerpoint",
                        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                        "text/plain",                                  // Tệp văn bản
                        "video/mp4", "video/mpeg", "video/quicktime",  // Video
                        "audio/mpeg", "audio/wav",                     // Audio
                        "application/zip", "application/x-rar-compressed"  // Tệp nén
                    };

                    if (!allowedFileTypes.Contains(file.ContentType))
                    {
                        throw new Exception("ContentType is invalid");
                    }

                    BlobServiceClient _blobServiceClient = new BlobServiceClient(connectString);
                    var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                    // Đổi tên file tránh trùng lặp
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    var newName = $"{fileNameWithoutExt}_{DateTime.Now:ddMMyyHHmmss}{fileExtension}";

                    BlobClient blobClient = blobContainerClient.GetBlobClient(newName);

                    using (var stream = file.OpenReadStream())
                    {
                        var blobHttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = file.ContentType // Đặt Content-Type chính xác
                        };

                        await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
                    }

                    return blobClient.Uri.AbsoluteUri;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                var containerName = _config["BlobService:Container"];
                var connectString = _config["BlobService:ConnectionString"];

                BlobServiceClient _blobServiceClient = new BlobServiceClient(connectString);
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
                var result = await blobClient.DeleteIfExistsAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<BlobOject?> DownloadAsync(string blobFilename)
        {
            try
            {
                var containerName = _config["BlobService:Container"];
                var connectString = _config["BlobService:ConnectionString"];

                BlobServiceClient _blobServiceClient = new BlobServiceClient(connectString);
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                BlobClient blobClient = blobContainerClient.GetBlobClient(blobFilename);

                if (await blobClient.ExistsAsync())
                {
                    var data = await blobClient.OpenReadAsync();
                    Stream blobContent = data;

                    var content = await blobClient.DownloadContentAsync();
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    return new BlobOject { Content = blobContent, Name = name, ContentType = contentType };
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error downloading file: {e.Message}");
                return null;
            }

            
        }

    }
}
