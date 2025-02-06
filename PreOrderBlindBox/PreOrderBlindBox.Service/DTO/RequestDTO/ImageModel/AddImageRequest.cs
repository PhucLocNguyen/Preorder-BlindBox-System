using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.ImageModel
{
    public class AddImageRequest
    {
        public List<IFormFile>? Files { get; set; }
        public int? BlindBoxId { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<bool?> IsMainImage { get; set; }
    }
}
