using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.BlindBoxModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlindBoxController : ControllerBase
    {
        private readonly IBlindBoxService _blindboxService;
        public BlindBoxController(IBlindBoxService blindBoxService)
        {
            _blindboxService = blindBoxService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllActiveBlindBox([FromQuery] PaginationParameter parameter)
        {
            var model = await _blindboxService.GetAllActiveBlindBoxAsync(parameter);
            if (model == null)
            {
                return NotFound();
            }
            var metadata = new
            {
                model.TotalCount,
                model.PageSize,
                model.CurrentPage,
                model.TotalPages,
                model.HasNext,
                model.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(model);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            var item = await _blindboxService.GetBlindBoxByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RequestCreateBlindBox request)
        {
            var result = await _blindboxService.CreateBlindBox(request);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] RequestUpdateBlindBox request)
        {
            var resultUpdate = await _blindboxService.UpdateBlindBox(id, request);
            if (resultUpdate)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
