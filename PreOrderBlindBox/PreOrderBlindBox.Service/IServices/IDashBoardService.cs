using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Services.DTO.RequestDTO.DashBoardModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.DashBoardModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
	public interface IDashBoardService
	{
		public Task<List<RevenueDto>> GetRevenuedByTime(DateTime? fromDate, DateTime? toDate);
		public Task<List<ResponseTopThreeCampaign>> GetTopThreeCampaign(DateTime? fromDate, DateTime? toDate);
		public Task<ResponseOrdersComparedLastMonth> GetOrderInFormationComparedToLastMonth();
		public Task<List<ResponseDashboardOrderByYear>> GetOrderByYear(int year);
	}
}
