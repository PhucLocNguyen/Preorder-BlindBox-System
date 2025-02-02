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
		private readonly IConfiguration _configuration;

		public AuthenController(IUserService userService, IConfiguration configuration)
		{
			_userService = userService;
			_configuration = configuration;
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

		[HttpPost("login")]
		public async Task<IActionResult> LoginWithEmailAndPassword([FromBody] RequestLoginByEmailAndPassword loginModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.LoginByEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
					_ = int.TryParse(_configuration["JwtSettings:TokenValidityInMinutes"], out int tokenValidityInMinutes);

					var cookieOptions = new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.Strict,
						Expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes - 5)
					};

					Response.Cookies.Append("accessToken", result.AccessToken, cookieOptions);

					return Ok(new { Message = "Login successful" });
				}
				return BadRequest(new { Message = "Login failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			try
			{
				Response.Cookies.Delete("accessToken");
				return Ok(new { Message = "Logout successful" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = "Logout failed" });
			}
		}

	}
}
