using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartByUserID(int userID) 
        {
            var listCart = await _cartService.GetAllCartByCustomerID(userID);
            return Ok(listCart);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantityInCart(RequestCreateCart requestCreateCart)
        {
            return Ok(await _cartService.ChangeQuantityOfCartByCustomerID(requestCreateCart));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCartItem(RequestCreateCart requestCreateCart)
        {
            return Ok(await _cartService.CreateCart(requestCreateCart));
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetPriceInCart([FromRoute]int userID)
        {
            return Ok(await _cartService.IdentifyPriceForCartItem(userID));
        }
    }
}
