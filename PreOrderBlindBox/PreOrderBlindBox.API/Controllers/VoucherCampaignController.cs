using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.VoucherCampaignModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VoucherCampaignController : ControllerBase
	{
		private readonly IVoucherCampaignService _voucherCampaignService;

		public VoucherCampaignController(IVoucherCampaignService voucherCampaignService)
		{
			_voucherCampaignService = voucherCampaignService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateVoucherCampaign(RequestCreateVoucherCompaign voucerCompaign)
		{
			try
			{
				var result = await _voucherCampaignService.CreateVoucherCampaignAsync(voucerCompaign);
				if (result > 0)
				{
					return Ok(new { Message = "Create voucher campaign successfully" });
				}
				return BadRequest(new { Message = "Create voucher campaign failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpDelete("{voucherCampaignId}")]
		public async Task<IActionResult> DeleteVoucherCampaign(int voucherCampaignId)
		{
			try
			{
				var result = await _voucherCampaignService.DeleteVoucherCampaignAsync(voucherCampaignId);
				if (result > 0)
				{
					return Ok(new { Message = "Deleted voucher campaign successfully" });
				}
				return BadRequest(new { Message = "Delete voucher campaign failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

		[HttpPut("{voucherCampaignId}")]
		public async Task<IActionResult> UpdateVoucherCampaign(int voucherCampaignId, [FromBody] RequestUpdateVoucherCampaign voucherCampaign)
		{
			try
			{
				int result = await _voucherCampaignService.UpdateVoucherCampaignAsync(voucherCampaignId, voucherCampaign);
				if (result > 0)
				{
					return Ok(new { Message = "Voucher campaign update successful" });
				}
				return BadRequest(new { Message = "Voucher campaign update failed" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = $"{ex.Message}" });
			}
		}

	}
}
