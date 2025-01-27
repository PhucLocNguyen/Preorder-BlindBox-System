using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetailsByOrderID(int orderID, [FromQuery] PaginationParameter paginationParameter)
        {
            return Ok(await _orderDetailService.GetAllOrderDetailsByOrderID(paginationParameter, orderID));
        }
    }
}
