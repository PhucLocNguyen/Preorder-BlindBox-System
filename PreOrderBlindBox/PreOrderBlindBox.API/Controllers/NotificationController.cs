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

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetAllNotification([FromRoute]int userID,[FromQuery]PaginationParameter paginationParameter) 
        {
            try
            {
                return Ok(await _notificationService.GetAllNotificationByUserId(userID, paginationParameter));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationById([FromRoute]int notificationId)
        {
            try
            {
                var existingNoti = await _notificationService.GetNotificationById(notificationId);
                if (existingNoti != null)
                {
                    await _notificationService.MarkNotificationAsRead(notificationId);
                    return Ok(existingNoti);
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
