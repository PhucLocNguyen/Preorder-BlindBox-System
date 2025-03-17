using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.UserModel;
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

		[HttpGet("GetAllStaff")]
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

		[HttpPut("UpdateCustomerInformation")]
		public async Task<IActionResult> UpdateCustomerInformation([FromForm] RequestUpdateCustomerInformation customerInformation)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.UpdateCustomerInformation(customerInformation);
					if (result > 0)
					{
						return Ok("Customer information updated successfully");
					}
				}
				return BadRequest(new { Message = "Customer information update failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPut("UpdateStaffInformation/{staffId}")]
		public async Task<IActionResult> UpdateStaffInformation(int staffId, [FromForm] RequestUpdateStaffInformation staffInformation)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = await _userService.UpdateStaffInformation(staffInformation, staffId);
					if (result > 0)
					{
						return Ok("Staff information updated successfully");
					}
				}
				return BadRequest(new { Message = "Staff information update failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpDelete("{staffId}")]
		public async Task<IActionResult> DeleteAccountStaff(int staffId)
		{
			try
			{
				var result = await _userService.DeleteStaff(staffId);
				if (result > 0)
				{
					return Ok("Staff deleted successfully");
				}
				return BadRequest(new { Message = "Staff delete failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}
	}
}
