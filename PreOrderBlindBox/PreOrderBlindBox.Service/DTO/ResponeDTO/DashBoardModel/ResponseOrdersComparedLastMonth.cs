using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.DTO.ResponeDTO.DashBoardModel
{
	public class ResponseOrdersComparedLastMonth
	{
		public int CurrentMonthOrder { get; set; }
		public double PercentComparedLastMonth { get; set; }
	}
}
