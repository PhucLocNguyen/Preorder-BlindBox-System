using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Services.DTO.RequestDTO.DashBoardModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.DashBoardModel;
using PreOrderBlindBox.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Services
{
	public class DashBoardService : IDashBoardService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderDetailRepository _orderDetailRepository;

		public DashBoardService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
		{
			_orderRepository = orderRepository;
			_orderDetailRepository = orderDetailRepository;
		}

		public async Task<List<RevenueDto>> GetRevenuedByTime(DateTime? fromDate, DateTime? toDate)
		{
			if (fromDate == null || toDate == null)
			{
				throw new Exception("Invalid input time");
			}
			if (fromDate >= toDate)
			{
				throw new Exception("Invalid input time");
			}

			var listRevenue = await _orderRepository.GetListRevenueByTime(fromDate.Value, toDate.Value);

			var dateRange = Enumerable.Range(0, (toDate.Value.Date - fromDate.Value.Date).Days + 1)
							  .Select(offset => fromDate.Value.Date.AddDays(offset))
							  .ToList();

			var result = dateRange.GroupJoin(listRevenue, date => date, revenue => revenue.Date,
				(date, revenues) => new
				{
					Date = date,
					Revenues = revenues
				})
				.SelectMany(x => x.Revenues.DefaultIfEmpty(),
					(x, revenue) => new RevenueDto
					{
						Date = x.Date,
						TotalRevenue = revenue?.TotalRevenue ?? 0
					})
				.ToList();

			return result ?? new List<RevenueDto>();
		}

		public async Task<List<ResponseTopThreeCampaign>> GetTopThreeCampaign(DateTime? fromDate, DateTime? toDate)
		{
			if (fromDate == null || toDate == null)
			{
				throw new Exception("Invalid input time");
			}
			if (fromDate >= toDate)
			{
				throw new Exception("Invalid input time");
			}

			var listOrderDetail = await _orderDetailRepository.GetTopThreeCampaignByOrder(fromDate.Value, toDate.Value);

			var result = listOrderDetail.GroupBy(x => x.PreorderCampaignId)
				.Select(group => new ResponseTopThreeCampaign
				{
					PreOrderCampaignId = group.Key.Value,
					TotalOrder = group.Select(x => x.OrderId).Distinct().Count(),
					Name = group.FirstOrDefault()?.PreorderCampaign?.BlindBox?.Name
				})
			.OrderByDescending(x => x.TotalOrder).Take(3).ToList();

			return result ?? new List<ResponseTopThreeCampaign>();
		}


		public async Task<ResponseOrdersComparedLastMonth> GetOrderInFormationComparedToLastMonth()
		{
			DateTime currentTime = DateTime.Now;

			var listOrderCurrentMonth = await _orderRepository.GetListOrderByMonth(currentTime);
			var listOrderPreviousMonth = await _orderRepository.GetListOrderByMonth(currentTime.AddMonths(-1));

			double percentageChange = 0;

			if (listOrderPreviousMonth.Count != 0)
			{
				percentageChange = ((double)(listOrderCurrentMonth.Count - listOrderPreviousMonth.Count) / listOrderPreviousMonth.Count) * 100;
			}
			else
			{
				percentageChange = listOrderCurrentMonth.Count > 0 ? 100 : 0;
			}

			var result = new ResponseOrdersComparedLastMonth
			{
				CurrentMonthOrder = listOrderCurrentMonth.Count,
				PercentComparedLastMonth = percentageChange
			};

			return result;

		}

	}
}
