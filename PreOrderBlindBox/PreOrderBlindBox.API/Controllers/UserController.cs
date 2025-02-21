using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllStaff()
		{
			try
			{
				var result = await _userService.GetAllStaff();
				if (result != null)
				{
					return Ok(result);
				}
				return BadRequest("Cannot get staff list");
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetUserById(int userId)
		{
			try
			{
				var result = await _userService.GetUserById(userId);
				if (result != null)
				{
					return Ok(result);
				}
				return BadRequest("Cannot get user");
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}
	}
}
