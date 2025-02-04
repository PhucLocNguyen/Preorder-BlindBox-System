using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.NotificationResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface INotificationService
    {
        Task<ResponseNotification> CreatNotification(RequestCreateNotification requestCreateNotification);
        Task<Pagination<ResponseNotification>> GetAllNotificationByUserId(int userId, PaginationParameter paginationParameter);
        Task<ResponseNotification?> MarkNotificationAsRead(int notificationId);
        Task<ResponseNotification> GetNotificationById(int notificationId);
    }
}
