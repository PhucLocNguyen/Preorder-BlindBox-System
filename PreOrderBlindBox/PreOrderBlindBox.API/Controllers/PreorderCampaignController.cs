using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderCampaignModel;
using PreOrderBlindBox.Services.IServices;

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
            var preorderCampaign = await _preorderCampaignService.AddPreorderCampaignAsync(request);
            if (preorderCampaign != null)
            {
                return Ok(preorderCampaign);
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreorderCampaign(int id)
        {
            if (await _preorderCampaignService.DeletePreorderCampaign(id) == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("UpdatePreorderCapaign/{id}")]
        public async Task<IActionResult> UpdatePreorderCampaign(int id, [FromBody] UpdatePreorderCampaignRequest request)
        {
            var preorderCampaign = await _preorderCampaignService.UpdatePreorderCampaign(id, request);

            if (preorderCampaign == null)
            {
                return BadRequest();
            }
            return Ok(preorderCampaign);
        }
    }
}
