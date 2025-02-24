using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel;
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
        public List<CreateMilestoneWithCampaign> MilestoneRequests { get; set; }
    }

    public class CreateMilestoneWithCampaign
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
