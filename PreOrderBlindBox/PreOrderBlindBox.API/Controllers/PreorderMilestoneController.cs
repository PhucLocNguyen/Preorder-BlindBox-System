using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreatePreorderMilestone(CreatePreorderMilestoneRequest createPreorderMilestoneRequest)
        {
            try
            {
                var preorderMilestone = await _preorderMilestoneService.AddPreorderMilestoneAsync(createPreorderMilestoneRequest);

                if (preorderMilestone <= 0)
                {
                    return BadRequest();
                }

                return Ok(preorderMilestone);
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
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeletePreorderCampaign(int id)
        {
            if (await _preorderMilestoneService.DeletePreorderMilestone(id) == false)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("UpdatePreorderMilestone/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePreorderMilestone(int id, [FromBody] UpdatePreorderMilestoneRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedMilestone = await _preorderMilestoneService.UpdatePreorderMilestone(id, request);

                if (updatedMilestone <= 0)
                {
                    return BadRequest();
                }

                return Ok(updatedMilestone);
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
