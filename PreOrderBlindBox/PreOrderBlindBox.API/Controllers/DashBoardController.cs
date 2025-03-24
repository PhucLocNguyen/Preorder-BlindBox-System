using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.DashBoardModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Services;

namespace PreOrderBlindBox.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DashBoardController : ControllerBase
	{
		private readonly IDashBoardService _dashBoardService;

		public DashBoardController(IDashBoardService dashBoardService)
		{
			_dashBoardService = dashBoardService;
		}

		[HttpGet("GetRevenueByTime")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetRevenueByTime(DateTime fromDate, DateTime toDate)
		{
			try
			{
				var result = await _dashBoardService.GetRevenuedByTime(fromDate, toDate);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpGet("GetTopThreeCampaign")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetTopThreeCampaign(DateTime fromDate, DateTime toDate)
		{
			try
			{
				var result = await _dashBoardService.GetTopThreeCampaign(fromDate, toDate);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpGet("last-month-comparison")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetOrderInFormationComparedToLastMonth()
		{
			try
			{
				var result = await _dashBoardService.GetOrderInFormationComparedToLastMonth();
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpGet("monthly-report/{year}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetOrderByYear(int year)
		{
			try
			{
				var result = await _dashBoardService.GetOrderByYear(year);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

	}
}
