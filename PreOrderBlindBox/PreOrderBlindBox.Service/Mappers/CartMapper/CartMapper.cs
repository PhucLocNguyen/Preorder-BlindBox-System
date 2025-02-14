using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;

namespace PreOrderBlindBox.Services.Mappers.CartMapper
{
    public static class CartMapper
    {
        public static Cart toCartEntity(this RequestCreateCart requestCreateCart,int userId)
        {
            return new Cart()
            {
                PreorderCampaignId = requestCreateCart.PreorderCampaignId,
                UserId = userId,
                Quantity = requestCreateCart.Quantity,
                CreateDate = DateTime.Now,
                IsDeleted = false
            };
        }

        public static ResponseCart toCartResponse(this Cart cart)
        {
            return new ResponseCart()
            {
                PreorderCampaignId = cart.PreorderCampaignId,
                Quantity = cart.Quantity,
            };
        }
    }
}
