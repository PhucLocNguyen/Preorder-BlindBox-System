using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.DashBoardModel
{
	public class ResponseTopThreeCampaign
	{
		public int PreOrderCampaignId { get; set; }
		public int TotalOrder { get; set; }
		public string Name { get; set; }
	}
}
