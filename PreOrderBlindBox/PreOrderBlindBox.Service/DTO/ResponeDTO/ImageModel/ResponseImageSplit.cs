using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.ImageModel
{
    public class ResponseImageSplit
    {
        public ResponseImageModel MainImage { get; set; }
        public List<ResponseImageModel> GalleryImages { get; set; }
    }
}
