using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.IServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

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

		[HttpGet("{orderID}")]
		public async Task<IActionResult> GetAllOrderDetailsByOrderID([FromRoute] int orderID, [FromQuery] PaginationParameter paginationParameter)
		{
			try
			{
				var itemResult = await _orderDetailService.GetAllOrderDetailsByOrderID(paginationParameter, orderID);
				return Ok(itemResult);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = (ex.Message) });
			}

		}
	}
}
