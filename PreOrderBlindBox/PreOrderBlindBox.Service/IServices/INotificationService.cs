﻿using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.IServices
{
    public interface INotificationService
    {
        Task CreatNotification(RequestCreateNotification requestCreateNotification);
    }
}
