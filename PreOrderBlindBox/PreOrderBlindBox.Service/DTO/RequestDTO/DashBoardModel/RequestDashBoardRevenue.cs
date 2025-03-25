using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.RequestDTO.DashBoardModel
{
	public class RequestDashBoardRevenue
	{
		[Required(ErrorMessage = "Input FromDate")]
		public DateTime FromDate { get; set; }

		[Required(ErrorMessage = "Input ToDate")]
		public DateTime ToDate { get; set; }
	}
}
