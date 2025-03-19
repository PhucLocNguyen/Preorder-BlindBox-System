using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.UserMapper
{
    public static class UserMapper
    {
        public static ResponseUserBankingInformation toUserBankingInformation(this User request)
        {
            return new ResponseUserBankingInformation()
            {

                Address = request.Address,
                BankName = request.BankName,
                Email = request.Email,
                FullName = request.FullName,
                BankAccountNumber = request.BankAccountNumber,
                UserId = request.UserId,
                Phone = request.Phone,
                Thumbnail= request.Thumbnail,
            };
        }
    }
}
