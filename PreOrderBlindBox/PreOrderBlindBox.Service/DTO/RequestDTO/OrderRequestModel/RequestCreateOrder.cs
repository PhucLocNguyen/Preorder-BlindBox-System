using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel
{
    public class RequestCreateOrder
    {
        // Con số đầu là preorderCampaign, số sau là voucherCampaign
        public Dictionary<int, int>? UserVoucherIdForPreorderCampaign { get; set; } = new Dictionary<int, int>();

        public decimal? Amount { get; set; }
        public decimal? DiscountMoney { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverPhone { get; set; }

        public string ReceiverAddress { get; set; }

        public string Status { get; set; } = "Watting";

		public RequestCreateCart? RequestCreateCart { get; set; }
    }
}
