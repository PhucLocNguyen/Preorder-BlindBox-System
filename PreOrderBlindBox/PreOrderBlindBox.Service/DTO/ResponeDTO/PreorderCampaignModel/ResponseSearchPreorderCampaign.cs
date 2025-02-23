using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.PreorderCampaignModel
{
    public class ResponseSearchPreorderCampaign
    {
        public int PreorderCampaignId { get; set; }
        public int? BlindBoxId { get; set; }
        public string Slug { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public bool IsDeleted { get; set; }
        public ResponseBlindBox BlindBox { get; set; }

        // Thêm 2 thuộc tính cho khoảng giá
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
    }
}
