using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel
{
    public class FilterPreorderCampaignRequest
    {
        public bool isEndingSoon { get; set; }
        public bool isNewlyLaunched { get; set; }
        public bool isTrending {  get; set; }
    }
}
