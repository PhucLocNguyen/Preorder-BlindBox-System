﻿using PreOrderBlindBox.Data.Commons;
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
        Task<bool> CreateNotification(List<RequestCreateNotification> requestCreateNotifications);
        Task<Pagination<ResponseNotification>> GetAllNotificationByUserId(int userId, PaginationParameter paginationParameter);
        Task<ResponseNotification?> MarkNotificationAsRead(int notificationId);
        Task<bool> MarkAllNotificationsAsRead();
        Task<ResponseNotification> GetNotificationById(int notificationId);
		Task<int> CountNotificationIsNotRead(int userId);
	}
}
