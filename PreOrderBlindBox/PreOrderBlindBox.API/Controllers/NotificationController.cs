using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                var listNotification = await _notificationService.GetAllNotificationByUserId(userID, paginationParameter);
				var metadata = new
				{
					listNotification.TotalCount,
					listNotification.PageSize,
					listNotification.CurrentPage,
					listNotification.TotalPages,
					listNotification.HasNext,
					listNotification.HasPrevious,
				};

				Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
				return Ok(listNotification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = (ex.Message) });
            }
        }

        [HttpGet("{notificationId}")]
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

		[HttpGet("unread-count")]
		public async Task<IActionResult> CountNotificationIsNotRead()
		{
			try
			{
				int userID = _currentUserService.GetUserId();
                var countResult = await _notificationService.CountNotificationIsNotRead(userID);
				return Ok(countResult);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = (ex.Message) });
			}

		}

		[HttpGet("mark-read-all")]
		public async Task<IActionResult> MarkAllNotificationsAsRead()
		{
			try
			{
				var isSuccess = await _notificationService.MarkAllNotificationsAsRead();
				if(isSuccess)
					return Ok(new { Message = "All notifications are read" });
				return BadRequest(new { Message = "Something wrong when mark all notifications read" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = (ex.Message) });
			}

		}
	}
}
