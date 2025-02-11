using PreOrderBlindBox.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel
{
    public class CreatePreorderCampaignRequest
    {
        public int? BlindBoxId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PreorderCampaignType Type { get; set; }
    }
}
