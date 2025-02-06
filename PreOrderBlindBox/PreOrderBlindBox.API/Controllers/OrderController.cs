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
            try
            {
                var listOrder = await _orderService.GetAllOrder(pagination);
                return Ok(listOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int orderId)
        {
            try
            {
                var existingOrder = await _orderService.GetOrderById(orderId);
                if (existingOrder != null)
                {
                    return Ok(existingOrder);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(RequestCreateOrder requestCreateOrder)
        {
            try
            {
                var itemResult = await _orderService.CreateOrder(requestCreateOrder);
                if (itemResult != null) return Ok(new { Message = "Create order successfully " });
                return BadRequest(new { Message = "Create order failed " });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
            
        }

    }
}
