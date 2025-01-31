using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.MailModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.UserModel;
using PreOrderBlindBox.Services.Helpers;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.Service.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRoleRepository _roleRepository;
		private readonly IMailService _mailService;

		public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IRoleRepository roleRepository, IMailService mailService)
		{
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_roleRepository = roleRepository;
			_mailService = mailService;
		}

		public async Task<bool> ConfirmEmailByTokenAsync(string confirmToken)
		{
			if (String.IsNullOrEmpty(confirmToken))
			{
				throw new ArgumentNullException(nameof(confirmToken));
			}
			var user = await _userRepository.GetUserByEmailConfirmToken(confirmToken);
			if (user == null)
			{
				throw new Exception("Confirm token invalid");
			}

			user.EmailConfirmToken = "";
			user.IsActive = true;
			user.IsEmailConfirm = true;

			await _userRepository.UpdateAsync(user);
			await _unitOfWork.SaveChanges();

			return true;
		}

		public Task LoginByEmailAndPassword(string email, string password)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> RegisterAccountAsync(RequestRegisterAccount registerAccount)
		{
			await _unitOfWork.BeginTransactionAsync();
			try
			{
				var checkEmail = await _userRepository.GetUserByEmailAsync(registerAccount.Email);
				if (checkEmail != null)
				{
					throw new Exception("Email already in use");
				}

				var checkRole = await _roleRepository.GetByIdAsync(registerAccount.RoleId);
				if (checkRole == null)
				{
					throw new Exception("Invalid role");
				}

				if (registerAccount.Password != registerAccount.ConfirmPassword)
				{
					throw new Exception("Password and confirm password not the same");
				}

				User newUser = new User()
				{
					FullName = registerAccount.FullName,
					Email = registerAccount.Email,
					Address = registerAccount.Address,
					IsEmailConfirm = false,
					IsActive = false,
					EmailConfirmToken = Guid.NewGuid().ToString(),
					RoleId = registerAccount.RoleId,
					Password = registerAccount.Password
				};

				await _userRepository.InsertAsync(newUser);

				//Gọi service để gửi email
				await _mailService.sendEmailAsync(new MailRequest()
				{
					ToEmail = newUser.Email,
					Subject = "[Pre-Order Blind Box] Confirm Account Email",
					Body = MailContent.ConfirmAccountEmail(newUser.FullName, newUser.EmailConfirmToken)
				});


				var result = await _unitOfWork.SaveChanges();
				await _unitOfWork.CommitTransactionAsync();
				return true;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}
	}
}
