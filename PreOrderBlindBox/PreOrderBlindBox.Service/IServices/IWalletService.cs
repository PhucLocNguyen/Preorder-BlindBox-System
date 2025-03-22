using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.WalletModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.WalletModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IWalletService
    {
        // Tao wallet cho nguoi dung, neu da tao roi thi khong tao nua
        public Task<bool> CreateWalletAsync(int userId);
        // Lay thong tin wallet cua nguoi dung
        public Task<ResponseShowWallet> GetWalletByUserIdAsync(int userId);
        // Rut tien tu wallet
        public Task<bool> WithdrawAsync(int userId, decimal amount);
        public Task<ResponseHistoryWallet> ShowDetailWalletAtTime(int userId, RequestShowHistoryWallet model);
    }
}
