using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.NotificationResponseModel;

namespace PreOrderBlindBox.Services.Mappers.NotificationMapper
{
    public static class NotificationMapper
    {
        public static Notification toNotificationEntity(this RequestCreateNotification requestCreateNotification)
        {
            return new Notification()
            {
                ReceiverId = requestCreateNotification.ReceiverId,
                Description = requestCreateNotification.Description,
                IsRead = false,
                IsDeleted = false,
                Title = requestCreateNotification.Title,
                CreatedDate = DateTime.Now,
            };
        }

        public static ResponseNotification toNotificationResponse(this Notification notification)
        {
            return new ResponseNotification()
            {
                Description = notification.Description,
                IsRead = notification.IsRead,
                Title = notification.Title,
            };
        }
    }
}
