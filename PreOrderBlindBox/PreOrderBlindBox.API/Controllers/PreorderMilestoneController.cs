using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.DTO.RequestDTO.PreorderMilestoneModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Services;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreorderMilestoneController : ControllerBase
    {
        private readonly IPreorderMilestoneService _preorderMilestoneService;

        public PreorderMilestoneController(IPreorderMilestoneService preorderMilestoneService)
        {
            _preorderMilestoneService = preorderMilestoneService;
        }

        [HttpGet("GetAllPreorderMilestoneByPreorderCampaignID")]
        public async Task<IActionResult> GetAllPreorderMilestoneByPreorderCampaignID([FromQuery] int id)
        {
            var result = await _preorderMilestoneService.GetAllPreorderMilestoneByPreorderCampaignID(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPreorderMilestoneById(int id)
        {
            var preorderMilestone = await _preorderMilestoneService.GetPreorderMilestoneById(id);
            if (preorderMilestone == null)
            {
                return NotFound();
            }
            return Ok(preorderMilestone);
        }

        [HttpPost("CreatePreorderMilestone")]
        public async Task<IActionResult> CreatePreorderMilestone(CreatePreorderMilestoneRequest createPreorderMilestoneRequest)
        {
            var preorderMilestone = await _preorderMilestoneService.AddPreorderMilestoneAsync(createPreorderMilestoneRequest);

            if (preorderMilestone == null)
            {
                return BadRequest();
            }

            return Ok(preorderMilestone);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePreorderCampaign(int id)
        {
            if (await _preorderMilestoneService.DeletePreorderMilestone(id) == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("UpdatePreorderMilestone/{id}")]
        public async Task<IActionResult> UpdatePreorderMilestone(int id, [FromBody] UpdatePreorderMilestoneRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedMilestone = await _preorderMilestoneService.UpdatePreorderMilestone(id, request);

            if (updatedMilestone == null)
            {
                return NotFound($"Milestone with ID {id} not found.");
            }

            return Ok(updatedMilestone);
        }
    }
}
