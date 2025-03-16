using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Service.Services;
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
		[HttpPost("RegisterCustomer")]
		public async Task<IActionResult> CreateUserByEmailAndPassword(RequestRegisterAccount registerModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.RegisterAccountAsync(registerModel);
					if (result > 0)
					{
						return Ok(new { Message = "Please confirm your email" });
					}
				}
				return BadRequest(new { Message = "Failed to create user" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
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
				return BadRequest(new { Message = $"{ex.Message}" });
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

					/*_ = int.TryParse(_configuration["JwtSettings:TokenValidityInMinutes"], out int tokenValidityInMinutes);

					var cookieOptions = new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.None,
						Expires = DateTime.Now.AddMinutes(tokenValidityInMinutes - 5),
						Path = "/"
					};

					Response.Cookies.Append("accessToken", result.AccessToken, cookieOptions);*/

					return Ok(result);
				}
				return BadRequest(new { Message = "Login failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
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

		[HttpGet("CurrentAccountRole")]
		public async Task<IActionResult> GetCurrentAccountRole()
		{
			try
			{
				var result = await _userService.GetCurrentAccountRole();
				return Ok(result);

			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPost("ChangePassword")]
		public async Task<IActionResult> ChangePassword([FromBody] RequestChangePassword changePassword)
		{
			try
			{
				var result = await _userService.ChangePassword(changePassword);
				if (result > 0)
				{
					return Ok(new { Message = "Password change successful" });
				}
				return BadRequest(new { Message = "Password change failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword([FromBody] RequestForgotPassword forgotPassword)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.ForgotPasswordForCustomer(forgotPassword);
					if (result > 0)
					{
						return Ok(new { Message = "Check your email to continue changing your password." });
					}
				}
				return BadRequest(new { Message = "Password change failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPost("AddNewPassword")]
		public async Task<IActionResult> AddNewPasswordForCustomer([FromBody] RequestAddNewPassword addNewPassword)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.AddNewPasswordForCustomer(addNewPassword);
					if (result > 0)
					{
						return Ok(new { Message = "Password changed successfully" });
					}
				}
				return BadRequest(new { Message = "Password change failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPost("RegisterStaff")]
		public async Task<IActionResult> CreateStaffByEmailAndPassword(RequestRegisterAccount registerModel)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.RegisterStaffAccountAsync(registerModel);
					if (result == true)
					{
						return Ok(new { Message = "Account created successfully" });
					}
				}
				return BadRequest(new { Message = "Failed to create user" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPost("LoginGoogle")]
		public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
		{
			try
			{
				var result = await _userService.LoginByGoogle(credential);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

	}
}
