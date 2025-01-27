using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.NotificationMapper;

namespace PreOrderBlindBox.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Notification> CreatNotification(RequestCreateNotification requestCreateNotification)
        {
            try
            {
                var NotificationEntity = requestCreateNotification.toNotificationEntity();
                await _notificationRepository.InsertAsync(NotificationEntity);
                await _unitOfWork.SaveChanges();
                return NotificationEntity;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong when create notification",ex);
            }
        }

        public async Task<Pagination<Notification>> GetAllNotificationByUserId(int userId, PaginationParameter paginationParameter)
        {
            var listNotification = await _notificationRepository.GetAll(filter: x => x.ReceiverId == userId,
                pagination: paginationParameter);
            return new Pagination<Notification>(listNotification, listNotification.Count, paginationParameter.PageIndex, paginationParameter.PageSize);
        }

        public async Task<Notification> GetNotificationById(int notificationId)
        {
            return await _notificationRepository.GetByIdAsync(notificationId);
        }

        public async Task<Notification> IsNotificationReaded(int notificationId, bool isRead)
        {
            var existingNoti = await _notificationRepository.GetByIdAsync(notificationId);
            if (existingNoti != null)
            {
                existingNoti.IsRead = isRead;
            }
            return existingNoti;
        }
    }
}
