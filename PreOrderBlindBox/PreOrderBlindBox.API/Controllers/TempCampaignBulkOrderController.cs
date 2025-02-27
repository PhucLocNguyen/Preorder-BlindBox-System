using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.IServices;
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
        public async Task<IActionResult> GetAllOrders([FromQuery] PaginationParameter pagination, [FromQuery] string? searchKeyWords, [FromQuery] string orderBy = "increase")
        {
            try
            {
                var listOrder = await _tempCampaignBulkOrderService.GetAllOrder(pagination, searchKeyWords, orderBy);
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
    }
}
