using Microsoft.EntityFrameworkCore;
using PreOrderBlindBox.Data.DBContext;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Repositories
{
	public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
	{
		public OrderDetailRepository(Preorder_BlindBoxContext context) : base(context)
		{
		}

		public async Task<List<OrderDetail>> GetTopThreeCampaignByOrder(DateTime fromDate, DateTime toDate)
		{
			return await _context.OrderDetails.Include(x => x.Order).Include(x => x.PreorderCampaign).ThenInclude(pc => pc.BlindBox)
				.Where(x => x.Order.CreatedDate.Date >= fromDate && x.Order.CreatedDate.Date <= toDate).ToListAsync();
		}
	}
}
