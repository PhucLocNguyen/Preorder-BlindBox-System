using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.UserModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenController : ControllerBase
	{
		private readonly IUserService _userService;

		public AuthenController(IUserService userService)
		{
			_userService = userService;
		}
		[HttpPost("register")]
		public async Task<IActionResult> CreateUserByEmailAndPassword(RequestRegisterAccount registerModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.RegisterAccountAsync(registerModel);
					if (result == true)
					{
						return Ok(new { Message = "User created successfully" });
					}
				}
				return BadRequest(new { Message = "Failed to create user" });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("confirm-email")]
		public async Task<IActionResult> ConfirmEmail(string confirmToken)
		{
			try
			{
				var result = await _userService.ConfirmEmailByTokenAsync(confirmToken);
				if (result)
				{
					return Ok(new { Message = "Email confirmation successful" });
				}
				return NotFound(new { Message = "Email confirmation failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
