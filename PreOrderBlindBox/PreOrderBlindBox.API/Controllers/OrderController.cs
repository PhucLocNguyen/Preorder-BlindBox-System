using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: api/<OrderController>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery]PaginationParameter pagination )
        {
            var listOrder = await _orderService.GetAllOrder(pagination);
            return  Ok(listOrder);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderById([FromQuery] int id)
        {
            var existingOrder = await _orderService.GetOrderById(id);
            if(existingOrder != null)
            {
                return Ok(existingOrder);
            }
            return NotFound();  
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(RequestCreateOrder requestCreateOrder)
        {
            await _orderService.CreateOrder(requestCreateOrder);
            return Ok();
        }

    }
}
