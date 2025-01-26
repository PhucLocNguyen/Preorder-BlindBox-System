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
        [HttpPost("CreatePaymentUrlMomo")]
        public async Task<IActionResult> CreateDepositUrl([FromBody] RequestDepositWallet request)
        {
            //int userId = _currentUserService.GetUserId();
            //if (userId == null)
            //{
            //    return Unauthorized();
            //}
            int userId = 3;

            var paymentInformation = await _paymentSerivce.CreatePaymentInMomoAsync(userId, request.Amount);
            if (paymentInformation != null)
            {
                return Ok(paymentInformation);
            }
            return BadRequest();
        }

        [HttpPost("DepositConfirmFromMomo")]
        public async Task<IActionResult> DepositConfirmFromMomo([FromBody] RequestMomoConfirm request)
        {
          
            if (await _walletService.DepositAsync(request))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("CreatePaymentUrlVnpay")]
        public async Task<IActionResult> CreateDepositUrlVnpay([FromBody] RequestDepositWallet request)
        {
            //int userId = _currentUserService.GetUserId();
            //if (userId == null)
            //{
            //    return Unauthorized();
            //}
            int userId = 3;

            var paymentInformation = await _paymentSerivce.CreatePaymentInVnPayAsync(userId, request.Amount);
            if (paymentInformation != null)
            {
                return Ok(paymentInformation);
            }
            return BadRequest();
        }
        [HttpGet("paymentVnPayCallBack")]
        public async Task<IActionResult> PaymentVnPayCallBack()
        {
          
            IQueryCollection requestQueryString = Request.Query;
            
            var result = await _paymentSerivce.VerifySignatureFromVnPay(requestQueryString);
            if (result.ResponseCode=="00")
            {
                return Redirect(_configuration["Vnpay:RedirectUrl"]);
            }
          

            return BadRequest();
        }

    }
}
