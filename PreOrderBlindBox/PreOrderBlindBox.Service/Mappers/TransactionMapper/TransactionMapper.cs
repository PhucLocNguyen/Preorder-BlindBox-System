using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TempCampaignBulkOrderDetailModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using PreOrderBlindBox.Services.Mappers.WalletMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.TransactionMapper
{
    public static class WalletMapper 
    {
        public static ResponsePendingWithdraw toReposonePendingWithdrawTransaction(this Transaction  request)
        {
            return new ResponsePendingWithdraw()
            {
                CreatedDate = request.CreatedDate,
                TransactionId = request.TransactionId,
                Status = request.Status,
                Type = request.Type,
                Description = request.Description,
                Money = request.Money,
                Wallet = request.Wallet.toResponseShowWallet(),

            };
        }
    }
}
