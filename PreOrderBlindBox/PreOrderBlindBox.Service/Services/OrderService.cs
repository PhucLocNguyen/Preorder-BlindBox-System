using Azure;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<Pagination<Order>> GetAllOrder(PaginationParameter page)
        {
            throw new NotImplementedException();
        }

        /*public async Task<Pagination<Order>> GetAllOrder(PaginationParameter? page)
{
   var listOrderDb = await _orderRepository.GetAll(pagination: page, includes: x=> x.Customer);

   return; 

}*/

        public Task<Order> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
