using PreOrderBlindBox.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel
{
    public class RequestTransactionCreateModel
    {
        public string Description { get; set; }

        public TypeOfTransactionEnum Type { get; set; }

        public decimal Money { get; set; }

        public decimal? BalanceAtTime { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? WalletId { get; set; }

        public int? OrderId { get; set; }
		public int? TempCampaignBulkOrderId { get; set; }

	}
}
