using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.UserVoucherModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserVouchersController : ControllerBase
	{
		private readonly IUserVoucherService _userVoucherService;

		public UserVouchersController(IUserVoucherService userVoucherService)
		{
			_userVoucherService = userVoucherService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateUserVoucherByVoucherCampaignId(RequestCreateUserVoucher userVoucher)
		{
			try
			{
				var result = await _userVoucherService.CreateUserVoucherAsync(userVoucher);
				if (result != null)
				{
					return Ok(result);
				}
				return BadRequest(new { Message = "Get voucher failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPut]
		public async Task<IActionResult> UseVoucher(RequestUpdateUserVoucher updateUserVoucher)
		{
			try
			{
				var result = await _userVoucherService.UpdateUserVoucherAsync(updateUserVoucher);
				if (result > 0)
				{
					return Ok(new { Message = "Use voucher successfully" });
				}
				return BadRequest(new { Message = "Voucher use failedk" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpDelete("{userVoucherId}")]
		public async Task<IActionResult> DeleteUserVoucher(int userVoucherId)
		{
			try
			{
				var result = await _userVoucherService.DeleteUserVoucherAsync(userVoucherId);
				if (result > 0)
				{
					return Ok(new { Message = "Deleted successfully" });
				}
				return BadRequest(new { Message = "Delete failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllUserVoucerCanUse()
		{
			try
			{
				var listUserVoucher = await _userVoucherService.GetAllUserVoucher();
				return Ok(listUserVoucher);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpGet("{userVoucherId}")]
		public async Task<IActionResult> GetUserVoucherById(int userVoucherId)
		{
			try
			{
				var userVoucher = await _userVoucherService.GetUserVoucherById(userVoucherId);
				return Ok(userVoucher);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

	}
}
