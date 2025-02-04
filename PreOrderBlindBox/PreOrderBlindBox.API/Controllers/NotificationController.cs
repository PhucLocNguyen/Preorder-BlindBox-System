using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotification(int userID,[FromQuery]PaginationParameter paginationParameter) 
        {
            return Ok(await _notificationService.GetAllNotificationByUserId(userID, paginationParameter));
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            var existingNoti = await _notificationService.GetNotificationById(notificationId);
            if (existingNoti != null)
            {
                await _notificationService.MarkNotificationAsRead(notificationId);
                return Ok(existingNoti);
            }
            return NotFound();
        }
    }
}
