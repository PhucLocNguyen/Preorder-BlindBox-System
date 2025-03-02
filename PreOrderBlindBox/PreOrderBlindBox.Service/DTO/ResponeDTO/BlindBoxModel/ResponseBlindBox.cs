using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.ImageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel
{
    public class ResponseBlindBox
    {
        public int BlindBoxId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal ListedPrice { get; set; }
        public string Size { get; set; }

        public DateTime CreatedAt { get; set; }

        public ResponseImageSplit Images { get; set; }
    }
}
