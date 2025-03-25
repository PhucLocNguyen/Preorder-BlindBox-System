using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.WalletModel
{
    public class RequestDepositWallet
    {
        [Range(typeof(decimal), "10000", "500000000", ErrorMessage = "Số tiền phải lớn hơn hoặc bằng 10,000 đồng và nhỏ hơn hoặc bằng 50 triệu VNĐ")]
        public decimal Amount { get; set; }
    }
}
