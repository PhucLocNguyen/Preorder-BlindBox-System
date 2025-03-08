using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.CartRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.OrderRequestModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;
        public OrderController(IOrderService orderService, ICurrentUserService currentUserService)
        {
            _orderService = orderService;
            _currentUserService = currentUserService;
        }
        // GET: api/<OrderController>
        //[Authorize(Roles = "3")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] PaginationParameter pagination, [FromQuery] string? searchKeyWords, [FromQuery] string orderBy = "increase")
        {
            try
            {
                var listOrder = await _orderService.GetAllOrder(pagination, searchKeyWords, orderBy);
                var metadata = new
                {
                    listOrder.TotalCount,
                    listOrder.PageSize,
                    listOrder.CurrentPage,
                    listOrder.TotalPages,
                    listOrder.HasNext,
                    listOrder.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(listOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet("staff/{orderId}")]
        public async Task<IActionResult> GetOrderByIdForStaff([FromRoute] int orderId)
        {
            try
            {
                var existingOrder = await _orderService.GetOrderByIdForStaff(orderId);
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

        [HttpGet("customer/{orderId}")]
        public async Task<IActionResult> GetOrderByIdForCustomer([FromRoute] int orderId)
        {
            try
            {
                var existingOrder = await _orderService.GetOrderByIdForCustomer(orderId);
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
                await _orderService.CreateOrder(requestCreateOrder);
                return Ok(new { Message = "Create order successfully " });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }

        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateStatusOfOrder(RequestUpdateOrder requestUpdateOrder,[FromRoute] int orderId)
        {
            try
            {
                var itemResult = await _orderService.UpdateStatusOfOrder(orderId, requestUpdateOrder);
                return Ok(itemResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet("view-history-orders")]
		public async Task<IActionResult> ViewOrderHistory([FromQuery] PaginationParameter pagination)
		{
            try
            {
				var items = await _orderService.OrderHistory(pagination);
                return Ok(items);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = (ex.Message) });
			}

		}
	}
}
