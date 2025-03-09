using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Services;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TempCampaignBulkOrderController : ControllerBase
    {
        private readonly ITempCampaignBulkOrderService _tempCampaignBulkOrderService;
        private readonly ICurrentUserService _currentUserService;
        public TempCampaignBulkOrderController(ITempCampaignBulkOrderService tempCampaignBulkOrderService)
        {
            _tempCampaignBulkOrderService = tempCampaignBulkOrderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTempOrders([FromQuery] PaginationParameter pagination, [FromQuery] string? searchKeyWords, [FromQuery] string orderBy = "increase")
        {
            try
            {
                var listOrder = await _tempCampaignBulkOrderService.GetAllTempOrder(pagination, searchKeyWords, orderBy);
                var metadata = new
                {
                    listOrder.TotalCount,
                    listOrder.PageSize,
                    listOrder.CurrentPage,
                    listOrder.TotalPages,
                    listOrder.HasNext,
                    listOrder.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(listOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpPost("{preorderCampaignId}/accept")]
        public async Task<IActionResult> AcceptTempOrder([FromRoute]int preorderCampaignId)
        {
            try
            {
                var result = await _tempCampaignBulkOrderService.AcceptTempOrder(preorderCampaignId);
                return Ok(new { Message = "Accept successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpPost("{preorderCampaignId}/reject")]
        public async Task<IActionResult> RejectTempOrder([FromRoute] int preorderCampaignId)
        {
            try
            {
                var result = await _tempCampaignBulkOrderService.RejectTempOrder(preorderCampaignId);
                return Ok(new { Message = "Reject successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet("view-history-temp-orders")]
        public async Task<IActionResult> ViewTempOrderHistory([FromQuery] PaginationParameter pagination)
        {
            try
            {
                var items = await _tempCampaignBulkOrderService.TempOrderHistory(pagination);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }

        }

        [HttpGet("customer/{tempOrderId}")]
        public async Task<IActionResult> GetTempOrderById([FromRoute] int tempOrderId)
        {
            try
            {
                var existingTempOrder = await _tempCampaignBulkOrderService.GetTempOrderByIdForCustomer(tempOrderId);
                if (existingTempOrder != null)
                {
                    return Ok(existingTempOrder);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }

        }
    }
}
