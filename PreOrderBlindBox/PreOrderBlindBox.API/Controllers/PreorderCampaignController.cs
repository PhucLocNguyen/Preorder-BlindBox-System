using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.Enum;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Services;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreorderCampaignController : ControllerBase
    {
        private readonly IPreorderCampaignService _preorderCampaignService;

        public PreorderCampaignController(IPreorderCampaignService preorderCampaignService)
        {
            _preorderCampaignService = preorderCampaignService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPreorderCampaign([FromQuery] PaginationParameter pagination)
        {
            var result = await _preorderCampaignService.GetAllActivePreorderCampaign(pagination);
            var metadata = new
            {
                result.TotalCount,
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
                result.HasNext,
                result.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearcgPreorderCampaign([FromQuery] PreorderCampaignSearchRequest searchRequest, [FromQuery] PaginationParameter pagination)
        {
            try
            {
                var preorderCampaign = await _preorderCampaignService.SearchPreorderCampaignAsync(searchRequest, pagination);
                if (preorderCampaign == null)
                {
                    return NotFound(new { message = "Preorder campaign not found." });
                }
                var metadata = new
                {
                    preorderCampaign.TotalCount,
                    preorderCampaign.PageSize,
                    preorderCampaign.CurrentPage,
                    preorderCampaign.TotalPages,
                    preorderCampaign.HasNext,
                    preorderCampaign.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(preorderCampaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPreorderCampaignById(int id)
        {
            var preorderCampaign = await _preorderCampaignService.GetPreorderCampaignAsyncById(id);
            if (preorderCampaign == null)
            {
                return NotFound();
            }
            return Ok(preorderCampaign);
        }

        [HttpGet("campaign/{slug}")]
        public async Task<IActionResult> GetPreorderCampaignBySlug(string slug)
        {
            var preorderCampaign = await _preorderCampaignService.GetPreorderCampaignBySlugAsync(slug);
            if (preorderCampaign == null)
            {
                return NotFound();
            }
            return Ok(preorderCampaign);
        }

        [HttpPost("CreatePreorderCampaign")]
        public async Task<IActionResult> CreatePreoderCampaign([FromBody]CreatePreorderCampaignRequest request)
        {
            try
            {
                var preorderCampaign = await _preorderCampaignService.AddPreorderCampaignAsync(request);
                if (preorderCampaign > 0)
                {
                    return Ok(preorderCampaign);
                }
                return BadRequest();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreorderCampaign(int id)
        {
            try
            {
                var result = await _preorderCampaignService.DeletePreorderCampaign(id);
                if (!result)
                {
                    return BadRequest(new { message = "Preorder campaign not found or already deleted." });
                }
                return Ok(new { message = "Preorder campaign deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the preorder campaign.", error = ex.Message });
            }
        }

        [HttpPut("UpdatePreorderCampaign/{id}")]
        public async Task<IActionResult> UpdatePreorderCampaign(int id, [FromBody] UpdatePreorderCampaignRequest request)
        {
            try
            {
                var preorderCampaign = await _preorderCampaignService.UpdatePreorderCampaign(id, request);

                if (preorderCampaign <= 0)
                {
                    return NotFound(new { message = "Preorder campaign not found." });
                }

                return Ok(preorderCampaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("CancelPreorderCampaign/{id}")]
        public async Task<IActionResult> CancelPreorderCampaign(int id, [FromBody] CancelPreorderCampaignRequest request)
        {
            try
            {
                var preorderCampaign = await _preorderCampaignService.CancelPreorderCampaign(id, request);

                if (preorderCampaign <= 0)
                {
                    return NotFound(new { message = "Preorder campaign not found." });
                }

                return Ok(preorderCampaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("CreatePreorderCampaignWithMilestone")]
        public async Task<IActionResult> CreatePreoderCampaignWithMilestone([FromBody] CreatePreorderCampaignRequest requestCampaign)
        {
            try
            {
                var preorderCampaign = await _preorderCampaignService.AddCampaignWithMilestonesAsync(requestCampaign);
                if (preorderCampaign)
                {
                    return Ok(preorderCampaign);
                }
                return BadRequest();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

    }
}
