using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.WalletModel
{
    public class ResponseHistoryWallet
    {
        public decimal BalanceAtTime { get; set; }
        public decimal TotalWithdrawAtTime{ get; set; }
        public decimal TotalDepositAtTime { get; set; }
        public decimal TotalPayAtTime { get; set; }
        public decimal TotalRefundAtTime { get; set; }
        public decimal TotalWithdrawPendingAtTime { get; set; }

    }
}
