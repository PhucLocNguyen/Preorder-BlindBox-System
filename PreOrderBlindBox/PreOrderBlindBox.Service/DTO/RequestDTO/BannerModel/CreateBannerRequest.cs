using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.BannerModel
{
    public class CreateBannerRequest
    {
        public string Title { get; set; }
        public string CallToActionUrl { get; set; }
        public int? Priority { get; set; }
        public IFormFile? File { get; set; }
    }
}
