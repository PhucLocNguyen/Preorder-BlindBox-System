using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetail>> GetAllOrderDetailsWithOrderID(PaginationParameter? page, int orderId);
    }
}
