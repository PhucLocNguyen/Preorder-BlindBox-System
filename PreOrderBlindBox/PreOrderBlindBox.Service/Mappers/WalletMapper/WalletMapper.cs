using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderDetailRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.TempCampaignBulkOrderDetailModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.WalletModel;
using PreOrderBlindBox.Services.Mappers.UserMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.WalletMapper
{
    public static class WalletMapper 
    {
        public static ResponseShowWallet toResponseShowWallet(this Wallet request)

        {
            return new ResponseShowWallet()
            {
                CreatedDate = request.CreatedDate,
                Balance = request.Balance,
                UserBankingInformation = request.Users.FirstOrDefault().toUserBankingInformation(),
                WalletId = request.WalletId 
            };
        }
    }
}
