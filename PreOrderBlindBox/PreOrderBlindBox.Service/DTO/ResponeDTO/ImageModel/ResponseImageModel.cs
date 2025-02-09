using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.ImageModel
{
    public class ResponseImageModel
    {
        public int ImageId { get; set; }

        public string Url { get; set; }

        public bool IsMainImage { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
