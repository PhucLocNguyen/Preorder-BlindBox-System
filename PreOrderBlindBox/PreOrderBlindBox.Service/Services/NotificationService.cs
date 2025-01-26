using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestDTO;
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
        public async Task CreatNotification(RequestCreateNotification requestCreateNotification)
        {
            try
            {
                var NotificationEntity = requestCreateNotification.toNotificationEntity();
                await _notificationRepository.InsertAsync(NotificationEntity);
                await _unitOfWork.SaveChanges();

            }catch (Exception ex)
            {
                throw ex;   
            }
        }
    }
}
