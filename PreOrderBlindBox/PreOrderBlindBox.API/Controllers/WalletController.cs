using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.MomoModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.VnPayModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.WalletModel;
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
        public WalletController(IWalletService walletService, IPaymentSerivce paymentSerivce, ICurrentUserService currentUserService)
        {
            _walletService = walletService;
            _paymentSerivce = paymentSerivce;
            _currentUserService = currentUserService;
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

        //[HttpPost("request")]
        //public async Task<IActionResult> RequestVNPay(int transactionid, decimal price)
        //{
        //    try
        //    {
        //        string url = _paymentSerivce.RequestVNPay(transactionid, price, HttpContext);
        //        return Ok(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("return")]
        //public async Task<IActionResult> VNPayReturn([FromQuery] RequestVnPayCreate model)
        //{
        //    try
        //    {
        //        var result = await _VNPayService.ReturnFromVNPay(model);
        //        if (result.Status == false)
        //        {
        //            return BadRequest(result);
        //        }
        //        else
        //        {
        //            return Redirect("");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
    }
}
