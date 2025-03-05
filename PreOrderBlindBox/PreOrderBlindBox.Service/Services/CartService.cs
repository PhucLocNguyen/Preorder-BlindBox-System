using AutoMapper;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Service.Services;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
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
		private readonly IPreorderCampaignRepository _preorderCampaignRepository;
		private readonly IUserVoucherService _userVoucherService;
		
		public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork,
			IPreorderMilestoneService preorderMilestoneService,
			IOrderDetailService orderDetailService,
			ICurrentUserService currentUserService,
			IMapper mapper, IBlindBoxService blindBoxService,
			IPreorderCampaignRepository preorderCampaignRepository,
			IUserVoucherService userVoucherService
			)
		{
			_cartRepository = cartRepository;
			_unitOfWork = unitOfWork;
			_preorderMilestoneService = preorderMilestoneService;
			_orderDetailService = orderDetailService;
			_currentUserService = currentUserService;
			_mapper = mapper;
			_blindBoxService = blindBoxService;
			_preorderCampaignRepository = preorderCampaignRepository;
			_userVoucherService = userVoucherService;
		}

		public async Task<Cart> ChangeQuantityOfCartByCustomerID(RequestCreateCart requestCreateCart)
		{
			var preorderCampaign = await _preorderCampaignRepository.GetDetailPreorderCampaignById((int)requestCreateCart.PreorderCampaignId);
			if (preorderCampaign.Type.Equals("TimedPricing"))
			{
				var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)requestCreateCart.PreorderCampaignId);
				int quantityForMilestone = preorderMilestones.Sum(x => x.Quantity);
				bool isEnoughQuantity = quantityForMilestone >= (preorderCampaign.PlacedOrderCount + requestCreateCart.Quantity);
				if (!isEnoughQuantity)
				{
					throw new Exception("The quantity has exceeded the limit allowed by that campaign.");
				}
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
					existingCart = await ChangeQuantityOfCartByCustomerID(requestCreateCart);
				}
				else
				{
					await _unitOfWork.BeginTransactionAsync();
					var preorderCampaign = await _preorderCampaignRepository.GetDetailPreorderCampaignById((int)requestCreateCart.PreorderCampaignId);
					if (preorderCampaign.Type.Equals("TimedPricing"))
					{
						var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)requestCreateCart.PreorderCampaignId);
						int quantityForMilestone = preorderMilestones.Sum(x => x.Quantity);

						bool isEnoughQuantity = quantityForMilestone >= (preorderCampaign.PlacedOrderCount + requestCreateCart.Quantity);

						if (!isEnoughQuantity)
						{
							throw new Exception("The quantity has exceeded the limit allowed by that campaign.");
						}
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
			return await _cartRepository.GetAll(filter: x => (x.UserId == customerID) && (x.IsDeleted == false), orderBy: x=>x.OrderBy(x=>x.PreorderCampaignId));
		}

		public async Task<Cart> GetCartByCustomerIDAndCampaignID(int customerID, int campaignID)
		{
			return (await _cartRepository.GetAll(filter:
				x => (x.PreorderCampaignId == campaignID)
				&& (x.IsDeleted == false) &&
				(x.UserId == customerID))).FirstOrDefault();
		}

		public async Task<List<ResponseCartWithVoucher>> IdentifyPriceForCartItem(int customerID, Dictionary<int, int> UserVoucherIdForPreorderCampaign, RequestCreateCart? requestCreateCart)
		{
			List<ResponseCart> cartItemPrices = new List<ResponseCart>();
			List<ResponseCartWithVoucher> cartItemPricesAfterVoucher = new List<ResponseCartWithVoucher>();
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
				
				var preorderCampaign = await _preorderCampaignRepository.GetDetailPreorderCampaignById((int)cart.PreorderCampaignId);
				var cartItem = new Cart()
				{
					CartId = cart.CartId,
					CreateDate = cart.CreateDate,
					IsDeleted = cart.IsDeleted,
					PreorderCampaignId = cart.PreorderCampaignId,
					Quantity = cart.Quantity,
					UserId = cart.UserId,
				};
				var orderDetailsQuantity = preorderCampaign.PlacedOrderCount;
				var preorderMilestones = await _preorderMilestoneService.GetAllPreorderMilestoneByCampaignID((int)cartItem.PreorderCampaignId);
				var blindBox = await _blindBoxService.GetBlindBoxByIdAsync((int)preorderCampaign.BlindBoxId);
				if (preorderCampaign.Type.Equals("TimedPricing"))
				{
					foreach (var milestone in preorderMilestones)
					{
						//Tính số lượng còn lại bao nhiêu cái đối với từng mốc 
						int remainQuantity = await _preorderMilestoneService.CalculateRemainingQuantity(milestone.Quantity, (int)orderDetailsQuantity);
						if (remainQuantity == 0)
						{
							orderDetailsQuantity = orderDetailsQuantity - milestone.Quantity;
						}
						else
						{
							// Nếu số lượng còn lại trong mốc nhiều hơn số lượng khác hàng mua trong cart
							if (remainQuantity >= cartItem.Quantity)
							{
								cartItemPrices.Add(new ResponseCart()
								{
									BlindBox = blindBox,
									PreorderCampaignId = cartItem.PreorderCampaignId,
									UserId = cartItem.UserId,
									Price = milestone.Price,
									Quantity = cartItem.Quantity,
									Amount = milestone.Price * cartItem.Quantity
								});
								break;
							}//Nếu số lượng còn lại trong mốc ít hơn nhưng vẫn đủ hàng 
							else
							{
								cartItem.Quantity = cartItem.Quantity - remainQuantity;
								cartItemPrices.Add(new ResponseCart()
								{
									BlindBox = blindBox,
									PreorderCampaignId = cartItem.PreorderCampaignId,
									UserId = cartItem.UserId,
									Price = milestone.Price,
									Quantity = remainQuantity,
									Amount = milestone.Price * remainQuantity
								});
								orderDetailsQuantity = 0;
							}
						}
					}
				}
				else if (preorderCampaign.Type.Equals("BulkOrder"))
				{
					int runningSum = preorderMilestones[0].Quantity;
					for (int i = 1; i < preorderMilestones.Count; i++)
					{
						runningSum += preorderMilestones[i].Quantity;
						int remainQuantity = await _preorderMilestoneService.CalculateRemainingQuantity(runningSum, (int)orderDetailsQuantity);
						if (remainQuantity == 0)
						{
							orderDetailsQuantity -= runningSum;
						}
						else
						{
							if (remainQuantity >= cartItem.Quantity)
							{
								cartItemPrices.Add(new ResponseCart()
								{
									BlindBox = blindBox,
									PreorderCampaignId = cartItem.PreorderCampaignId,
									UserId = cartItem.UserId,
									Price = preorderMilestones[i - 1].Price,
									Quantity = cartItem.Quantity,
									Amount = preorderMilestones[i - 1].Price * cartItem.Quantity
								});
								break;
							}
							else
							{
								cartItem.Quantity = cartItem.Quantity - remainQuantity;
								cartItemPrices.Add(new ResponseCart()
								{
									BlindBox = blindBox,
									PreorderCampaignId = cartItem.PreorderCampaignId,
									UserId = cartItem.UserId,
									Price = preorderMilestones[i - 1].Price,
									Quantity = remainQuantity,
									Amount = preorderMilestones[i - 1].Price * remainQuantity
								});
								orderDetailsQuantity = 0;
							}
						}
						if (i == preorderMilestones.Count - 1 && cartItem.Quantity > 0)
						{
							cartItemPrices.Add(new ResponseCart()
							{
								BlindBox = blindBox,
								PreorderCampaignId = cartItem.PreorderCampaignId,
								UserId = cartItem.UserId,
								Price = preorderMilestones[i].Price,
								Quantity = cartItem.Quantity,
								Amount = preorderMilestones[i].Price * cartItem.Quantity
							});
						}
						runningSum = 0;
					}
				}
				if (UserVoucherIdForPreorderCampaign.ContainsKey((int)cart.PreorderCampaignId))
				{
					var userVoucher = await _userVoucherService.GetUserVoucherByVoucherCampaignId(UserVoucherIdForPreorderCampaign[(int)cart.PreorderCampaignId]);
					var amountUsingVoucher = cartItemPrices.Sum(x => x.Amount) * (userVoucher.PercentDiscount / 100);
					decimal discountMoney = (decimal)(amountUsingVoucher > userVoucher.MaximumMoneyDiscount ? userVoucher.MaximumMoneyDiscount : amountUsingVoucher) ;
					cartItemPricesAfterVoucher.Add(new ResponseCartWithVoucher()
					{
						responseCarts = cartItemPrices,
						UserVoucherId = userVoucher.UserVoucherId ,
						TempTotal = (decimal)cartItemPrices.Sum(x=>x.Amount),
						DiscountMoney = discountMoney,
						Total = (decimal)cartItemPrices.Sum(x => x.Amount) - discountMoney,
					}) ;
				}else
				{
					cartItemPricesAfterVoucher.Add(new ResponseCartWithVoucher()
					{
						responseCarts = cartItemPrices,
						UserVoucherId = 0,
						TempTotal = (decimal)cartItemPrices.Sum(x => x.Amount),
						DiscountMoney = 0,
						Total = (decimal)cartItemPrices.Sum(x => x.Amount) - 0,
					});
				}
				cartItemPrices = new List<ResponseCart>();
			}

			return cartItemPricesAfterVoucher;
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
