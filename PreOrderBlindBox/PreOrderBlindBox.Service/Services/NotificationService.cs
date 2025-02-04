using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.NotificationResponseModel;
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
        public async Task<ResponseNotification> CreatNotification(RequestCreateNotification requestCreateNotification)
        {
            try
            {
                var NotificationEntity = requestCreateNotification.toNotificationEntity();
                await _notificationRepository.InsertAsync(NotificationEntity);
                await _unitOfWork.SaveChanges();
                return NotificationEntity.toNotificationResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong when create notification",ex);
            }
        }

        public async Task<Pagination<ResponseNotification>> GetAllNotificationByUserId(int userId, PaginationParameter paginationParameter)
        {
            var listNotification = await _notificationRepository.GetAll(filter: x => x.ReceiverId == userId,
                pagination: paginationParameter);
            return new Pagination<ResponseNotification>(listNotification.Select(x => x.toNotificationResponse()).ToList(), listNotification.Count, paginationParameter.PageIndex, paginationParameter.PageSize);
        }

        public async Task<ResponseNotification> GetNotificationById(int notificationId)
        {
            return (await _notificationRepository.GetByIdAsync(notificationId)).toNotificationResponse();
        }

        public async Task<ResponseNotification?> MarkNotificationAsRead(int notificationId)
        {
            var existingNoti = await _notificationRepository.GetByIdAsync(notificationId);
            if (existingNoti == null)
            {
                return null;
            }
            existingNoti.IsRead = true;
            await _notificationRepository.UpdateAsync(existingNoti);
            await _unitOfWork.SaveChanges();

            return existingNoti.toNotificationResponse();
        }
    }
}
