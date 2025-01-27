using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
