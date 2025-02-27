using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.TempCampaignBulkOrderModel
{
    public class ResquestCreateTempCampaignBulkOrder
    {
        public int? UserVoucherId { get; set; }

        public decimal? Amount { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverPhone { get; set; }

        public string ReceiverAddress { get; set; }

        public string Status { get; set; } = "Pending";

        public RequestCreateCart? RequestCreateCart { get; set; }
    }
}
