using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurcusProject.CM.Helpers
{
    public interface IBlobService
    {
        Task<string?> UploadFile(IFormFile file);
        Task<bool> DeleteFile(string fileName);
        Task<BlobOject?> DownloadAsync(string blobFilename);
    }
}
