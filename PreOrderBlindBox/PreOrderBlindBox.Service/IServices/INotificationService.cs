using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface INotificationService
    {
        Task<Notification> CreatNotification(RequestCreateNotification requestCreateNotification);
        Task<Pagination<Notification>> GetAllNotificationByUserId(int userId, PaginationParameter paginationParameter);
        Task<Notification> IsNotificationReaded(int notificationId, bool isRead);
        Task<Notification> GetNotificationById(int notificationId);
    }
}
