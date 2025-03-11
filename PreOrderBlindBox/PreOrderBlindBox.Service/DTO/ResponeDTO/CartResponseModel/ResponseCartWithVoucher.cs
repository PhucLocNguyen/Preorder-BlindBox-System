using PreOrderBlindBox.Services.DTO.ResponeDTO.UserVouchersModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel
{
	public class ResponseCartWithVoucher
	{
		public List<ResponseCart> responseCarts { get; set; }

		public ResponseUserVoucher? UserVoucher { get; set; }

		public decimal TempTotal { get; set; }

		public decimal DiscountMoney { get; set; }

		public decimal Total { get; set; }
	}
}
