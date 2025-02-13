using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.DTO.ResponeDTO.TransactionModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ICurrentUserService _currentUserService;

        public TransactionController(ITransactionService transactionService, ICurrentUserService currentUserService)
        {
            _transactionService = transactionService;
        }
        [HttpGet("GetTransactionStatusVerifyUserPayment")]
        public async Task<IActionResult> GetTransactionStatusVerifyUserPayment([FromQuery] string transactionId)
        {
            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            ResponseTransactionResult transactionDetail = await _transactionService.GetDetailTransactionVerifyUser(transactionId, userId);

            if (transactionDetail == null)
            {
                return BadRequest();
            }
            return Ok(transactionDetail);
        }
        [HttpGet("GetListOfAllTransaction")]
        public async Task<IActionResult> GetListOfAllTransaction([FromQuery] PaginationParameter pagination)
        {
            var model = await _transactionService.GetListOfAllTransaction(pagination);
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

    }
}
