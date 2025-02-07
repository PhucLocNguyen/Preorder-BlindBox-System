using PreOrderBlindBox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel
{
    public class ResponseBlindBox
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Size { get; set; }

        public string Category { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    }
}
