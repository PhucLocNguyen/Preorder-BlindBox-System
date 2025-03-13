using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.PreorderCampaignModel
{
    public class ResponsePreorderCampaign
    {
        public int PreorderCampaignId { get; set; }
        public string Slug { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int? PlacedOrderCount { get; set; }
        public decimal PriceAtTime { get; set; }
        public int TotalQuantity { get; set; }
        public ResponseBlindBox BlindBox { get; set; }
    }
}
