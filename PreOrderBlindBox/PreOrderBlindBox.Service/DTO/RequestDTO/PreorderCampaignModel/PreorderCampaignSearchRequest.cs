using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Enum
{
    public class PreorderCampaignSearchRequest
    {
        public string? BlindBoxName { get; set; }
        public PreorderCampaignSortOrderEnum SortOrder { get; set; }
    }

    public class PreorderCampaignGetRequest
    {
        public PreorderCampaignType? Type { get; set; }
        public bool isEndingSoon { get; set; }
        public bool isNewlyLaunched { get; set; }
        public bool isTrending { get; set; }
    }
}
