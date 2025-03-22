using PreOrderBlindBox.Data.Commons;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.NotificationRequestModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.NotificationResponseModel;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Mappers.NotificationMapper;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.Services.Services
{
	public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(
            INotificationRepository notificationRepository, 
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService
            )
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

		public async Task<int> CountNotificationIsNotRead(int userId)
		{
            var result = await _notificationRepository.GetAll(filter: x=>x.IsRead == false && x.ReceiverId == userId);
			return result.Count();
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
            var listNotification = await _notificationRepository.GetAll(filter: x => x.ReceiverId == userId && x.IsDeleted == false,
                pagination: paginationParameter, orderBy: x=>x.OrderByDescending(t => t.CreatedDate));
            var listResponseNotification = listNotification.Select(x => x.toNotificationResponse()).ToList();
            var countItem = _notificationRepository.Count(x => x.ReceiverId == userId);
			return new Pagination<ResponseNotification>(listResponseNotification, countItem, paginationParameter.PageIndex, paginationParameter.PageSize);
        }

        public async Task<ResponseNotification> GetNotificationById(int notificationId)
        {
            var notificationById = await _notificationRepository.GetByIdAsync(notificationId);

			return (notificationById.toNotificationResponse());
        }

		public async Task<bool> MarkAllNotificationsAsRead()
		{
            await _unitOfWork.BeginTransactionAsync();
            try
            {
				var userId = _currentUserService.GetUserId();
                var allNotificationByUserId = await _notificationRepository.GetAll(filter: x=> x.ReceiverId == userId);
                foreach (var item in allNotificationByUserId)
                {
                    item.IsRead = true;
                    await _notificationRepository.UpdateAsync(item);
                }
                await _unitOfWork.SaveChanges();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
			catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
		}

		public async Task<ResponseNotification?> MarkNotificationAsRead(int notificationId)
        {
            var existingNoti = await _notificationRepository.GetByIdAsync(notificationId);
            existingNoti.IsRead = true;
            await _notificationRepository.UpdateAsync(existingNoti);
            await _unitOfWork.SaveChanges();

            return existingNoti.toNotificationResponse();
        }
    }
}
