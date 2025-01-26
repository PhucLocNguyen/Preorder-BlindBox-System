using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel
{
    public class CreatePreorderMilestoneRequest
    {
        public int? PreorderCampaignId { get; set; }
        public int MilestoneNumber { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
