using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.PreorderCampaignModel
{
    public class ResponsePreorderCampaignDetail
    {
        public int PreorderCampaignId { get; set; }
        public int? BlindBoxId { get; set; }
        public string Slug { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalQuantity { get; set; }
        public int? PlacedOrderCount { get; set; }
        public ResponseBlindBox BlindBox { get; set; }
    }
}
