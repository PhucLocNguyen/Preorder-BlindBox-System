using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.DTO.RequestDTO.TransactionRequestModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.WalletModel;
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
            _currentUserService = currentUserService;
        }
        [Authorize(Roles = "Customer")]
        [HttpGet("GetTransactionDetailVerifyUserPayment")]
        public async Task<IActionResult> GetTransactionDetailVerifyUserPayment([FromQuery] string transactionId)
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

        [Authorize(Roles = "Admin")]
        [HttpGet("GetListOfAllTransaction")]
        public async Task<IActionResult> GetListOfAllTransaction([FromQuery] RequestTransactionReportModel modelRequest)
        {
            var model = await _transactionService.GetListOfAllTransaction(modelRequest);
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

        [HttpGet("GetListOfAllTransactionByUser")]
        public async Task<IActionResult> GetListOfAllTransactionByUser([FromQuery] PaginationParameter pagination)
        {
            int userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var model = await _transactionService.GetListOfTransactionByUser(pagination, userId);
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
        [Authorize(Roles = "Admin")]
        [HttpGet("withdrawals")]
        public async Task<IActionResult> GetListOfWithdrawRequest([FromQuery] PaginationParameter paginationParameter)
        {
            int userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var model = await _transactionService.GetListPendingWithdrawRequest(paginationParameter);
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
        [Authorize(Roles = "Customer")]
        [HttpPost("withdrawals")]
        public async Task<IActionResult> CreateWithdrawMoneyFromWallet([FromBody] RequestDepositWallet model)
        {
            int userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            bool result = await _transactionService.CreateWithdrawRequest(userId, model.Amount);
            if (result == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("/withdrawals/{withdrawalId}/approval")]
        public async Task<IActionResult> ApproveWithdrawal([FromRoute] int withdrawalId)
        {
            bool result = await _transactionService.ConfirmWithdrawTransaction(withdrawalId);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
