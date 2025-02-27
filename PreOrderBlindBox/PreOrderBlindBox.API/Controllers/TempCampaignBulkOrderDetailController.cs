using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Services;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempCampaignBulkOrderDetailController : ControllerBase
    {
        private readonly ITempCampaignBulkOrderDetailService _tempCampaignBulkOrderDetailService;

        public TempCampaignBulkOrderDetailController(ITempCampaignBulkOrderDetailService tempCampaignBulkOrderDetailService)
        {
            _tempCampaignBulkOrderDetailService = tempCampaignBulkOrderDetailService;
        }

        [HttpGet("{tempCampaignBulkOrderIdTempCampaignBulkOrderId}")]
        public async Task<IActionResult> GetAllOrderDetailsByOrderID([FromRoute] int tempCampaignBulkOrderIdTempCampaignBulkOrderId, [FromQuery] PaginationParameter paginationParameter)
        {
            try
            {
                var itemResult = await _tempCampaignBulkOrderDetailService.GetAllOrderDetailsByOrderID(paginationParameter, tempCampaignBulkOrderIdTempCampaignBulkOrderId);
                return Ok(itemResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }

        }
    }
}
