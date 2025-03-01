using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel
{
    public class UpdatePreorderCampaignRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PreorderCampaignType Type { get; set; }
        public List<UpdatePreorderMilestoneRequest?> PreorderMilestoneRequests { get; set; }
    }
}
