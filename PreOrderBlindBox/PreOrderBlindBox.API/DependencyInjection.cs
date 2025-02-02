using Microsoft.AspNetCore.Mvc.Infrastructure;
using PreOrderBlindBox.Data.GenericRepository;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.Repositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Service.Services;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Services;
using PreOrderBlindBox.Services.Utils;

namespace PreOrderBlindBox.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddWebAPI(this IServiceCollection services)
		{

			//Add Scope for Repositories
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
			services.AddScoped<IBlindBoxRepository, BlindBoxRepository>();
			services.AddScoped<ICartRepository, CartRepository>();
			services.AddScoped<IImageRepository, ImageRepository>();
			services.AddScoped<INotificationRepository, NotificationRepository>();
			services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IPreorderCampaignRepository, PreorderCampaignRepository>();
			services.AddScoped<IPreorderMilestoneRepository, PreorderMilestoneRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserVoucherRepository, UserVoucherRepository>();
			services.AddScoped<IVoucherCampaignRepository, VoucherCampaignRepository>();
			services.AddScoped<IWalletRepository, WalletRepository>();


			//Add Scope for Services
			services.AddScoped<IBlindBoxService, BlindBoxService>();
			services.AddScoped<ICartService, CartService>();
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<INotificationService, NotificationService>();
			services.AddScoped<IOrderDetailService, OrderDetailService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IPreorderCampaignService, PreorderCampaignService>();
			services.AddScoped<IPreorderMilestoneService, PreorderMilestoneService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<ITransactionService, TransactionService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IUserVoucherService, UserVoucherService>();
			services.AddScoped<IVoucherCampaignService, VoucherCampaignService>();
			services.AddScoped<IWalletService, WalletService>();
			services.AddScoped<IPaymentSerivce, PaymentService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddTransient<IMailService, MailService>();
			return services;
		}
	}
}
