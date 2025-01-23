using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Entities;
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
        public async Task<List<BlindBox>> GetAll()
        {
            return await _blindboxService.GetBlindBoxesAsync();
        }
    }
}
