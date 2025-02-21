using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.PreorderCampaignModel
{
    public class ResponsePreorderCampaign
    {
        public string Slug { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public ResponseBlindBoxWithMainImage BlindBox { get; set; }
    }
}
