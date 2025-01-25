using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost("CreatePaymentUrl")]
        public async Task<IActionResult> CreateDepositUrl([FromBody] RequestDepositWallet request)
        {
            int userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var paymentInformation = await _paymentSerivce.CreatePaymentAsync(userId, request.Amount);
            if (paymentInformation != null)
            {
                return Ok(paymentInformation);
            }
            return BadRequest();
        }

        [HttpPost("DepositConfirm")]
        public async Task<IActionResult> DepositConfirm([FromBody] RequestDepositWallet request)
        {
            int userId = _currentUserService.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (await _walletService.DepositAsync(userId, request.Amount))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
