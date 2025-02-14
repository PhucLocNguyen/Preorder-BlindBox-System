using PreOrderBlindBox.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel
{
    public class CancelPreorderCampaignRequest
    {
        public PreorderCampaignStatus Status { get; set; }
    }
}
