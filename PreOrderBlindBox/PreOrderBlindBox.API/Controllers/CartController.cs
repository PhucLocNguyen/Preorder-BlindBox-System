﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICurrentUserService _currentUserService;
		public CartController(ICartService cartService, ICurrentUserService currentUserService
            )
        {
            _cartService = cartService;
            _currentUserService = currentUserService;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartByUserID() 
        {
            try
            {
                int userID = _currentUserService.GetUserId();
                var listCart = await _cartService.GetAllCartByCustomerID(userID);
                return Ok(listCart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantityInCart(RequestCreateCart requestCreateCart)
        {
            try
            {
                var result = await _cartService.ChangeQuantityOfCartByCustomerID(requestCreateCart);
                if (result != null)
                    return Ok(new { Message = "Update quantity in cart successfully" });
                else return BadRequest("Something went wrong when input information");



			}
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCartItem(RequestCreateCart requestCreateCart)
        {
            try
            {
                var itemResult = await _cartService.CreateCart(requestCreateCart);
                if (itemResult != null) return Ok(new { Message = "Create cart successfully" });
				return BadRequest(new {Message = "Add new item failed"});

            }catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpPost("GetPriceInCart")]
        public async Task<IActionResult> GetPriceInCart([FromQuery]RequestCreateCart? requestCreateCart,[FromBody] Dictionary<int, int>? UserVoucherIdForPreorderCampaign)
        {
            try
            {
                int userID = _currentUserService.GetUserId();
                var itemResult = await _cartService.IdentifyPriceForCartItem(userID, UserVoucherIdForPreorderCampaign ,requestCreateCart);
                if (itemResult != null) return Ok(itemResult);
                return BadRequest(new { Message = "Something wrong when get price" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

		[HttpPut("ClearAllCart")]
		public async Task<IActionResult> ClearAllCart()
		{
			try
			{
				int userID = _currentUserService.GetUserId();
                if(userID == null)
                    return BadRequest(new { Message = "Login first" });
                var itemResult = await _cartService.UpdateStatusOfCartByCustomerID(userID);
				if (itemResult) return Ok(new { Message = "Clear cart successfully" });
				return BadRequest(new { Message = "Something wrong when get price" });

			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = (ex.Message) });
			}
		}
	}
}
