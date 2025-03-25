using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.IRepositories
{
	public interface IOrderRepository : IGenericRepository<Order>
	{
		public Task<List<RevenueDto>> GetListRevenueByTime(DateTime fromDate, DateTime toDate);
		public Task<List<Order>> GetListOrderByMonth(DateTime currentTime);
		public Task<List<Order>> GetListOrderByYear(int year);
	}
}
