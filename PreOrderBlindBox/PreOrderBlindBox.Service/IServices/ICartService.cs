using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface ICartService
    {
        Task CreateCart(RequestCreateCart requestCreateCart);
        Task<List<Cart>> GetAllCartByCustomerID(int customerID);
        Task UpdateStatusOfCartByCustomerID(int customerID);
    }
}
