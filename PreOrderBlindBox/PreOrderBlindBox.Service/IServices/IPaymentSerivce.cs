using Microsoft.AspNetCore.Http;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.VnPayModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.PaymentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IPaymentSerivce
    {
        public Task<string> CreatePaymentInMomoAsync(int userId,decimal amount);
        public Task<bool> VerifySignatureFromMomo(User user, RequestMomoConfirm request);

        public Task<string> CreatePaymentInVnPayAsync(int userId, decimal amount);
        public Task<ResponsePaymentResult> VerifySignatureFromVnPay(IQueryCollection request);
    }
}
