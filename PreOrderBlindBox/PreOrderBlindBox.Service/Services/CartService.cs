using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.BlindBoxModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.CartResponseModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.CartMapper;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPreorderMilestoneService _preorderMilestoneService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IBlindBoxService _blindBoxService;
        private readonly IPreorderCampaignService _preorderCampaignService;
        public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork,
            IPreorderMilestoneService preorderMilestoneService,
            IOrderDetailService orderDetailService,
            ICurrentUserService currentUserService,
            IMapper mapper, IBlindBoxService blindBoxService,
            IPreorderCampaignService preorderCampaignService
            )
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _preorderMilestoneService = preorderMilestoneService;
            _orderDetailService = orderDetailService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _blindBoxService = blindBoxService;
            _preorderCampaignService = preorderCampaignService;
        }

        public async Task<Cart> ChangeQuantityOfCartByCustomerID(RequestCreateCart requestCreateCart)
        {
            var orderDetailsQuantity = await _orderDetailService.GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc((int)requestCreateCart.PreorderCampaignId);
            var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)requestCreateCart.PreorderCampaignId);
            int quantityForMilestone = preorderMilestones.Sum(x => x.Quantity);

            bool isEnoughQuantity = quantityForMilestone >= (orderDetailsQuantity + requestCreateCart.Quantity);

            if (!isEnoughQuantity)
            {
                throw new Exception("The quantity has exceeded the limit allowed by that campaign.");
            }
            int userId = _currentUserService.GetUserId();
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingCart = await GetCartByCustomerIDAndCampaignID(userId, (int)requestCreateCart.PreorderCampaignId);
                if (existingCart != null)
                {
                    if (requestCreateCart.Quantity > 0)
                    {
                        existingCart.Quantity = requestCreateCart.Quantity;
                    }
                    else if (requestCreateCart.Quantity == 0)
                    {
                        existingCart.IsDeleted = true;
                    }
                    await _cartRepository.UpdateAsync(existingCart);
                    await _unitOfWork.SaveChanges();
                    await _unitOfWork.CommitTransactionAsync();
                }
                return existingCart;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception("Something went wrong when change quantity of cart", ex);
            }
        }

        public async Task<Cart> CreateCart(RequestCreateCart requestCreateCart)
        {

            int userId = _currentUserService.GetUserId();

            try
            {
                var existingCart = await GetCartByCustomerIDAndCampaignID(userId, (int)requestCreateCart.PreorderCampaignId);
                if (existingCart != null)
                {
                    requestCreateCart.Quantity = existingCart.Quantity + requestCreateCart.Quantity;
                    var orderDetailsQuantity = await _orderDetailService.GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc((int)requestCreateCart.PreorderCampaignId);
                    var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)requestCreateCart.PreorderCampaignId);
                    int quantityForMilestone = preorderMilestones.Sum(x => x.Quantity);

                    bool isEnoughQuantity = quantityForMilestone >= (orderDetailsQuantity + requestCreateCart.Quantity);

                    if (!isEnoughQuantity)
                    {
                        throw new Exception("The quantity has exceeded the limit allowed by that campaign.");
                    }
                    existingCart = await ChangeQuantityOfCartByCustomerID(requestCreateCart);

                }
                else
                {
                    await _unitOfWork.BeginTransactionAsync();
                    var orderDetailsQuantity = await _orderDetailService.GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc((int)requestCreateCart.PreorderCampaignId);
                    var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)requestCreateCart.PreorderCampaignId);
                    int quantityForMilestone = preorderMilestones.Sum(x => x.Quantity);

                    bool isEnoughQuantity = quantityForMilestone >= (orderDetailsQuantity + requestCreateCart.Quantity);

                    if (!isEnoughQuantity)
                    {
                        throw new Exception("The quantity has exceeded the limit allowed by that campaign.");
                    }
                    if (requestCreateCart.Quantity > 0)
                    {
                        existingCart = requestCreateCart.toCartEntity(userId);
                        await _cartRepository.InsertAsync(existingCart);
                        await _unitOfWork.SaveChanges();
                    }
                    else throw new Exception("Cart item quantity must be larger than 0");
                    await _unitOfWork.CommitTransactionAsync();
                }
                return existingCart;

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
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

        public async Task<List<ResponseCart>> IdentifyPriceForCartItem(int customerID, RequestCreateCart? requestCreateCart)
        {
            List<ResponseCart> cartItemPrices = new List<ResponseCart>();
            List<Cart> listCart = new List<Cart>();
            if (requestCreateCart.PreorderCampaignId == null)
            {
                listCart = await GetAllCartByCustomerID(customerID);

            }
            else
            {
                listCart.Add(requestCreateCart.toCartEntity(customerID));
            }
            foreach (var cart in listCart)
            {
                var orderDetailsQuantity = await _orderDetailService.GetQuantitesOrderDetailsByPreorderCampaignIDSortedByTimeAsc((int)cart.PreorderCampaignId);
                var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)cart.PreorderCampaignId);
                //Tính tổng số lượng có hàng có trong mốc đó 

                foreach (var milestone in preorderMilestones)
                {
                    var preorderCampaign = await _preorderCampaignService.GetPreorderCampaignAsyncById((int)cart.PreorderCampaignId);
                    var blindBox = await _blindBoxService.GetBlindBoxByIdAsync((int)preorderCampaign.BlindBoxId);
                    var blindBoxResponse = _mapper.Map<ResponseBlindBox>(blindBox);
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
                                BlindBox = blindBoxResponse,
                                PreorderCampaignId = cart.PreorderCampaignId,
                                UserId = cart.UserId,
                                Price = milestone.Price,
                                Quantity = cart.Quantity,
                                Amount = milestone.Price * cart.Quantity
                            });
                            break;
                        }//Nếu số lượng còn lại trong mốc ít hơn nhưng vẫn đủ hàng 
                        else
                        {
                            cart.Quantity = cart.Quantity - remainQuantity;
                            cartItemPrices.Add(new ResponseCart()
                            {
                                BlindBox = blindBoxResponse,
                                PreorderCampaignId = cart.PreorderCampaignId,
                                UserId = cart.UserId,
                                Price = milestone.Price,
                                Quantity = remainQuantity,
                                Amount = milestone.Price * remainQuantity
                            });
                            orderDetailsQuantity = 0;
                        }
                    }

                }
            }
            return cartItemPrices.OrderBy(x => x.PreorderCampaignId).ToList();
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
