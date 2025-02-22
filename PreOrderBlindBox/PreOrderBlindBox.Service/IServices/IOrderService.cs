using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.OrderResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface IOrderService
    {
        Task<Pagination<ResponseOrder>> GetAllOrder(PaginationParameter page, string? searchKeyWords);
        Task<ResponseOrder> GetOrderById(int id);
        Task<Order> CreateOrder(RequestCreateOrder requestCreateOrder,RequestCreateCart? requestCreateCart);
    }
}
