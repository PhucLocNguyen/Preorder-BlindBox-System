using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface ICartService
    {
        Task<Cart> CreateCart(RequestCreateCart requestCreateCart);
        Task<List<Cart>> GetAllCartByCustomerID(int customerID);
        Task<Cart> GetCartByCustomerIDAndCampaignID(int customerID, int campaignID);
        Task<bool> UpdateStatusOfCartByCustomerID(int customerID);
        Task<Cart> ChangeQuantityOfCartByCustomerID( RequestCreateCart requestUpdateCart);
        Task<List<ResponeCart>> IdentifyPriceForCartItem(int customerID);
    }
}
