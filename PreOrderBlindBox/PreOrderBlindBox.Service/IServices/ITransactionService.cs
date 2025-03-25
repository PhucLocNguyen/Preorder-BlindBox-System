using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface ITransactionService
    {
        public Task<ResponseTransactionResult> GetDetailTransactionVerifyUser(string transactionId, int userId);
        public Task<Pagination<ResponseTransactionResult>> GetListOfAllTransaction(RequestTransactionReportModel model);
        public Task<Pagination<ResponseTransactionResult>> GetListOfTransactionByUser(PaginationParameter paginationParameter, int userId);
        public Task<bool> CreateTransaction(RequestTransactionCreateModel model);
        public Task<bool> CreateWithdrawRequest(int userId, decimal money);
        public Task<bool> ConfirmWithdrawTransaction(int transactionId);
        public Task<Pagination<ResponsePendingWithdraw>> GetListPendingWithdrawRequest(PaginationParameter paginationParameter);
        public Task<ResponsePendingWithdraw> GetDetailPendingWithdraw(int transactionId);
    }
}
