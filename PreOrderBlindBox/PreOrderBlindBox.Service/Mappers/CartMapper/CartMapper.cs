using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Mappers.CartMapper
{
    public static class CartMapper
    {
        public static Cart toCartEntity(this RequestCreateCart requestCreateCart)
        {
            return new Cart()
            {
                PreorderCampaignId = requestCreateCart.PreorderCampaignId,
                UserId = requestCreateCart.UserId,
                Quantity = requestCreateCart.Quantity,
                CreateDate = DateTime.Now,
                IsDeleted = false
            };
        }

        public static ResponeCart toResponeCart(this Cart cart)
        {
            return new ResponeCart()
            {
                PreorderCampaignId = cart.PreorderCampaignId,
                Quantity = cart.Quantity,
            };
        }
    }
}
