using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.BlindBoxModel
{
    public class RequestCreateBlindBox
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public string listedPrice { get; set; }
        public string Size { get; set; }
        public IFormFile MainImage { get; set; }
        public List<IFormFile>? GalleryImages { get; set; }
    }
}
