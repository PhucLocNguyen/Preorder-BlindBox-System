using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.CartMapper;

namespace PreOrderBlindBox.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPreorderMilestoneService _preorderMilestoneService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork,
            IPreorderMilestoneService preorderMilestoneService,
            IOrderDetailService orderDetailService)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _preorderMilestoneService = preorderMilestoneService;
            _orderDetailService = orderDetailService;
        }

        public async Task<Cart> ChangeQuantityOfCartByCustomerID(RequestCreateCart requestUpdateCart)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingCart = await GetCartByCustomerIDAndCampaignID((int)requestUpdateCart.UserId, (int)requestUpdateCart.PreorderCampaignId);
                if (existingCart != null)
                {
                    existingCart.Quantity = requestUpdateCart.Quantity;
                    await _cartRepository.UpdateAsync(existingCart);
                    await _unitOfWork.SaveChanges();
                    await _unitOfWork.CommitTransactionAsync();
                }
                return existingCart;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Something went wrong when change quantity of cart",ex);
            }
        }

        public async Task<Cart> CreateCart(RequestCreateCart requestCreateCart)
        {
            try
            {
                var cartEntity = requestCreateCart.toCartEntity();
                await _cartRepository.InsertAsync(cartEntity);
                await _unitOfWork.SaveChanges();
                return cartEntity;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong when add blind box to cart", ex);
            }
        }

        public async Task<List<Cart>> GetAllCartByCustomerID(int customerID)
        {
            return await _cartRepository.GetAll(filter: x => (x.UserId == customerID) && (x.IsDeleted == false));
        }

        public async Task<Cart> GetCartByCustomerIDAndCampaignID(int customerID, int campaignID)
        {
            return (await _cartRepository.GetAll(filter:
                x => (x.PreorderCampaignId == campaignID)
                && (x.IsDeleted == false) &&
                (x.UserId == customerID))).FirstOrDefault();
        }

        public async Task<List<ResponseCart>> IdentifyPriceForCartItem(int customerID)
        {
            List<ResponseCart> cartItemPrices = new List<ResponseCart>();
            List<Cart> listCart = await GetAllCartByCustomerID(customerID);
            foreach (var cart in listCart)
            {
                var orderDetailsQuantity = await _orderDetailService.GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc((int)cart.PreorderCampaignId);
                var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)cart.PreorderCampaignId);
                //Tính tổng số lượng có hàng có trong mốc đó 
                int quantityForMilestone = preorderMilestones.Sum(x => x.Quantity);

                //Xem chiến dịch đó đã đủ số lượng mua chưa ( bao gồm cả đơn trong cart và đơn đã mua)
                bool isEnoughQuantity = quantityForMilestone >= (orderDetailsQuantity + cart.Quantity);

                foreach (var milestone in preorderMilestones)
                {
                    //Tính số lượng còn lại bao nhiêu cái đối với từng mốc 
                    int remainQuantity = await _preorderMilestoneService.CalculateRemainingQuantity(milestone.Quantity, orderDetailsQuantity);
                    if (remainQuantity == 0)
                    {
                        orderDetailsQuantity = orderDetailsQuantity - milestone.Quantity;
                    }
                    else
                    {
                        // Nếu số lượng còn lại trong mốc nhiều hơn số lượng khác hàng mua trong cart
                        if (remainQuantity >= cart.Quantity)
                        {
                            cartItemPrices.Add(new ResponseCart()
                            {
                                PreorderCampaignId = cart.PreorderCampaignId,
                                UserId = cart.UserId,
                                Price = milestone.Price,
                                Quantity = cart.Quantity,

                            });
                            break;
                        }//Nếu số lượng còn lại trong mốc ít hơn nhưng vẫn đủ hàng 
                        else if (remainQuantity < cart.Quantity && isEnoughQuantity)
                        {
                            var cartTemp = cart;
                            cartTemp.Quantity = remainQuantity;
                            cart.Quantity = cart.Quantity - remainQuantity;
                            cartItemPrices.Add(new ResponseCart()
                            {
                                PreorderCampaignId = cart.PreorderCampaignId,
                                UserId = cart.UserId,
                                Price = milestone.Price,
                                Quantity = remainQuantity
                            });

                        }//Nếu số lượng còn lại trong mốc ít hơn nhưng không đủ hàng 
                        else if (remainQuantity < cart.Quantity && !isEnoughQuantity)
                        {
                            var cartTemp = cart;
                            cartTemp.Quantity = remainQuantity;
                            cart.Quantity = cart.Quantity - remainQuantity;
                            // Thêm vào giá và số lượng sản phẩm ứng với mốc đó 
                            cartItemPrices.Add(new ResponseCart()
                            {
                                PreorderCampaignId = cart.PreorderCampaignId,
                                UserId = cart.UserId,
                                Price = milestone.Price,
                                Quantity = remainQuantity
                            });
                            // Do hết hàng nên số lượng sản phẩm còn lại không có giá
                            cartItemPrices.Add(new ResponseCart()
                            {
                                PreorderCampaignId = cart.PreorderCampaignId,
                                UserId = cart.UserId,
                                Price = -1,
                                Quantity = cart.Quantity
                            });
                            break;
                        }
                    }
                }
            }
            return cartItemPrices;
        }

        public async Task<bool> UpdateStatusOfCartByCustomerID(int customerID)
        {
            try
            {
                var listCartByCustomerID = await GetAllCartByCustomerID(customerID);
                foreach (var item in listCartByCustomerID)
                {
                    item.IsDeleted = true;
                    await _cartRepository.UpdateAsync(item);
                }
                await _unitOfWork.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong when updating with cart", ex);
            }

        }
    }
}
