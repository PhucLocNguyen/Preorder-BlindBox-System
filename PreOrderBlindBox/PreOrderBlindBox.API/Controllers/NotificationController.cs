using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUserService;
        public NotificationController(INotificationService notificationService, ICurrentUserService currentUserService)
        {
            _notificationService = notificationService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotification([FromQuery]PaginationParameter paginationParameter) 
        {
            try
            {
                int userID = _currentUserService.GetUserId();   
                return Ok(await _notificationService.GetAllNotificationByUserId(userID, paginationParameter));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet("notification/{notificationId}")]
        public async Task<IActionResult> GetNotificationById([FromRoute]int notificationId)
        {
            try
            {
                var existingNoti = await _notificationService.GetNotificationById(notificationId);
                if (existingNoti != null)
                {
                    var responseNotification = await _notificationService.MarkNotificationAsRead(notificationId);
                    return Ok(responseNotification);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
            
        }
    }
}
