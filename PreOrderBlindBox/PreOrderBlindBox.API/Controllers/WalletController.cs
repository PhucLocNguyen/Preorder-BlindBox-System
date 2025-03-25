using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.VnPayModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.WalletModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.PaymentModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.WalletModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IPaymentSerivce _paymentSerivce;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;
        public WalletController(IWalletService walletService, IPaymentSerivce paymentSerivce, ICurrentUserService currentUserService, IConfiguration configuration)
        {
            _walletService = walletService;
            _paymentSerivce = paymentSerivce;
            _currentUserService = currentUserService;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(await _walletService.GetWalletByUserIdAsync(userId));
        }
        [HttpGet("transactions")]
        public async Task<IActionResult> GetDetailWalletBySelectedTime([FromQuery] RequestShowHistoryWallet model)
        {
            var userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await _walletService.ShowDetailWalletAtTime(userId, model);
            return Ok(result);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("CreatePaymentUrlMomo")]
        public async Task<IActionResult> CreateDepositUrl([FromBody] RequestDepositWallet request)
        {
            int userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var paymentInformation = await _paymentSerivce.CreatePaymentInMomoAsync(userId, request.Amount);
            if (paymentInformation != null)
            {
                return Ok(paymentInformation);
            }
            return BadRequest();
        }

        [HttpGet("DepositConfirmFromMomo")]
        public async Task<IActionResult> DepositConfirmFromMomo([FromQuery] MomoReturnModel request)
        {

            var result = await _paymentSerivce.DepositMomoAsync(request);

            if (result.IsSuccess)
            {
                return Ok((new
                {
                    success = true,
                    message = result.Message
                }));
            }
            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }
        [Authorize(Roles = "Customer")]
        [HttpPost("CreatePaymentUrlVnpay")]
        public async Task<IActionResult> CreateDepositUrlVnpay([FromBody] RequestDepositWallet request)
        {
            int userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var paymentInformation = await _paymentSerivce.CreatePaymentInVnPayAsync(userId, request.Amount);
            if (paymentInformation != null)
            {
                return Ok(paymentInformation);
            }
            return BadRequest();
        }
        [HttpGet("DepositConfirmFromVnPay")]
        public async Task<IActionResult> DepositConfirmFromVnPay()
        {

            IQueryCollection requestQueryString = Request.Query;

            var result = await _paymentSerivce.DepositConfirmFromVnPay(requestQueryString);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }

    }
}
