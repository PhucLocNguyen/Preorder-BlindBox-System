using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IOrderService
    {
        Task<Pagination<Order>> GetAllOrder(PaginationParameter page);
        Task<Order> GetOrderById(int id);
    }
}
