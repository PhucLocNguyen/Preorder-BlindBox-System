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
            try
            {
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
                return Ok(await _cartService.ChangeQuantityOfCartByCustomerID(requestCreateCart));

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
                if (itemResult != null) return Ok(itemResult);
                return BadRequest(new {Message = "Add new item failed"});

            }catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetPriceInCart([FromRoute]int userID)
        {
            try
            {
                var itemResult = await _cartService.IdentifyPriceForCartItem(userID);
                if (itemResult != null) return Ok(itemResult);
                return BadRequest(new { Message = "Something wrong when get price" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }
    }
}
