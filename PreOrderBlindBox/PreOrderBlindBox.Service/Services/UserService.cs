using AutoMapper;
using Microsoft.Extensions.Configuration;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using PreOrderBlindBox.Data.UnitOfWork;
using PreOrderBlindBox.Services.DTO.RequestDTO.MailModel;
using PreOrderBlindBox.Services.DTO.RequestDTO.UserModel;
using PreOrderBlindBox.Services.DTO.ResponeDTO.UserModel;
using PreOrderBlindBox.Services.Helpers;
using PreOrderBlindBox.Services.IServices;
using PreOrderBlindBox.Services.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PreOrderBlindBox.Service.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRoleRepository _roleRepository;
		private readonly IMailService _mailService;
		private readonly IConfiguration _configuration;
		private readonly IWalletService _walletService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;

		public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IRoleRepository roleRepository, IMailService mailService,
			IConfiguration configuration, IWalletService walletService, ICurrentUserService currentUserService,
			IMapper mapper)
		{
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_roleRepository = roleRepository;
			_mailService = mailService;
			_configuration = configuration;
			_walletService = walletService;
			_currentUserService = currentUserService;
			_mapper = mapper;

		}

		public async Task<bool> ConfirmEmailByTokenAsync(string confirmToken)
		{
			if (String.IsNullOrEmpty(confirmToken))
			{
				throw new ArgumentNullException(nameof(confirmToken));
			}
			await _unitOfWork.BeginTransactionAsync();
			try
			{
				var user = await _userRepository.GetUserByEmailConfirmToken(confirmToken);
				if (user == null)
				{
					throw new Exception("Confirm token invalid");
				}

				user.EmailConfirmToken = "";
				user.IsActive = true;
				user.IsEmailConfirm = true;

				await _userRepository.UpdateAsync(user);
				// Tạo wallet cho User
				bool walletCreated = await _walletService.CreateWalletAsync(user.UserId);
				if (!walletCreated)
				{
					throw new Exception("Failed to create wallet");
				}

				await _unitOfWork.SaveChanges();
				await _unitOfWork.CommitTransactionAsync();
				return true;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}

		public async Task<ResponseLogin> LoginByEmailAndPasswordAsync(string email, string password)
		{
			var user = await _userRepository.GetUserByEmailAsync(email);

			if (user == null || !PasswordUtils.VerifyPassword(password, user.Password))
			{
				throw new Exception("Incorrect email or password");
			}

			if (user.IsActive == false)
			{
				throw new Exception("Inactive account");
			}

			if (user.IsEmailConfirm == false)
			{
				throw new Exception("Please confirm your email");
			}

			var accessToken = GenerateAccessToken(user);
			return new ResponseLogin
			{
				AccessToken = accessToken,
				RefreshToken = ""
			};

		}

		public async Task<int> RegisterAccountAsync(RequestRegisterAccount registerAccount)
		{
			await _unitOfWork.BeginTransactionAsync();
			try
			{
				var checkEmail = await _userRepository.GetUserByEmailAsync(registerAccount.Email);
				if (checkEmail != null)
				{
					throw new Exception("Email already in use");
				}

				if (registerAccount.Password != registerAccount.ConfirmPassword)
				{
					throw new Exception("Password and confirm password not the same");
				}

				var role = await _roleRepository.GetRoleByRoleName("Customer");

				User newUser = new User()
				{
					FullName = registerAccount.FullName,
					Email = registerAccount.Email,
					Address = registerAccount.Address,
					IsEmailConfirm = false,
					IsActive = false,
					EmailConfirmToken = Guid.NewGuid().ToString(),
					RoleId = role.RoleId,
					Password = PasswordUtils.HashPassword(registerAccount.Password)
				};

				await _userRepository.InsertAsync(newUser);

				//Gọi service để gửi email
				await _mailService.sendEmailAsync(new MailRequest()
				{
					ToEmail = newUser.Email,
					Subject = "[Pre-Order Blind Box] Xác nhận tài khoản",
					Body = MailContent.ConfirmAccountEmail(newUser.FullName, newUser.EmailConfirmToken)
				});


				var result = await _unitOfWork.SaveChanges();
				await _unitOfWork.CommitTransactionAsync();
				return result;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}
		}

		public string GenerateAccessToken(User user)
		{
			var claimList = new List<Claim>
			{
				new Claim(ClaimTypes.Role, user.Role.RoleName.ToString()),
				new Claim("userID", user.UserId.ToString())
			};
			var accessToken = GenerateJwtToken.AccessToken(claimList, _configuration);
			return new JwtSecurityTokenHandler().WriteToken(accessToken);
		}

		public async Task<ResponseCurrentAccountRole> GetCurrentAccountRole()
		{
			int userId = _currentUserService.GetUserId();
			var user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				throw new Exception("Account does not exist");
			}
			return new ResponseCurrentAccountRole { RoleName = user.Role.RoleName };
		}

		public async Task<int> ChangePassword(RequestChangePassword changePassword)
		{
			int userId = _currentUserService.GetUserId();
			var user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				throw new Exception("Account does not exist");
			}

			var checkPassword = PasswordUtils.VerifyPassword(changePassword.OldPassword, user.Password);
			if (!checkPassword)
			{
				throw new Exception("Old password is incorrect");
			}
			if (changePassword.OldPassword.Equals(changePassword.NewPassword))
			{
				throw new Exception("The new password cannot be the same as the old password.");
			}
			if (!changePassword.NewPassword.Equals(changePassword.ConfirmNewPassword))
			{
				throw new Exception("New password and confirm new password must be the same");
			}

			var newHashPassword = PasswordUtils.HashPassword(changePassword.NewPassword);
			user.Password = newHashPassword;
			await _userRepository.UpdateAsync(user);
			return await _unitOfWork.SaveChanges();

		}

		public async Task<int> ForgotPasswordForCustomer(RequestForgotPassword forgotPassword)
		{
			var user = await _userRepository.GetUserByEmailAsync(forgotPassword.Email);
			if (user == null)
			{
				throw new Exception("Account does not exist");
			}
			if (user.IsActive == false)
			{
				throw new Exception("Invalid account");
			}
			if (user.Role.RoleName.ToLower() != "customer")
			{
				throw new Exception("This account cannot change password");
			}

			await _unitOfWork.BeginTransactionAsync();
			try
			{
				string forgotPasswordToken = Guid.NewGuid().ToString();

				user.ForgotPasswordToken = forgotPasswordToken;
				user.ForgotPasswordTokenExpiry = DateTime.Now.AddHours(1);
				await _userRepository.UpdateAsync(user);

				// Gửi mail xác nhận cho người dùng
				await _mailService.sendEmailAsync(new MailRequest
				{
					ToEmail = user.Email,
					Subject = "[Pre-Order Blind Box] Yêu cầu lấy lại mật khẩu đăng nhập tài khoản",
					Body = MailContent.ForgotPasswordEmail(user.FullName, forgotPasswordToken, user.Email)
				});

				int result = await _unitOfWork.SaveChanges();
				await _unitOfWork.CommitTransactionAsync();
				return result;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackTransactionAsync();
				throw;
			}

		}

		public async Task<bool> RegisterStaffAccountAsync(RequestRegisterAccount registerStaffAccount)
		{
			//Kiểm tra người tạo có phải là admin không
			int userId = _currentUserService.GetUserId();
			var user = await _userRepository.GetByIdAsync(userId);
			if (user.Role.RoleName.ToLower() != "admin")
			{
				throw new Exception("You do not have permission to create account staff");
			}

			await _unitOfWork.BeginTransactionAsync();
			try
			{
				var checkEmail = await _userRepository.GetUserByEmailAsync(registerStaffAccount.Email);
				if (checkEmail != null)
				{
					throw new Exception("Email already in use");
				}

				if (registerStaffAccount.Password != registerStaffAccount.ConfirmPassword)
				{
					throw new Exception("Password and confirm password not the same");
				}
				var role = await _roleRepository.GetRoleByRoleName("Staff");

				User newUser = new User()
				{
					FullName = registerStaffAccount.FullName,
					Email = registerStaffAccount.Email,
					Address = registerStaffAccount.Address,
					IsEmailConfirm = true,
					IsActive = true,
					EmailConfirmToken = "",
					RoleId = role.RoleId,
					Password = PasswordUtils.HashPassword(registerStaffAccount.Password),
				};
				await _userRepository.InsertAsync(newUser);

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

		public async Task<List<ResponseUserInfomation>> GetAllStaff()
		{
			var role = await _roleRepository.GetRoleByRoleName("Staff");
			if (role == null)
			{
				throw new Exception("Invalid role staff");
			}
			var listStaff = await _userRepository.GetAllStaff(role.RoleId);
			return _mapper.Map<List<ResponseUserInfomation>>(listStaff);
		}

		public async Task<ResponseUserInfomation> GetUserById(int userId)
		{
			if (userId <= 0)
			{
				throw new ArgumentException("User ID must be greater than zero.");
			}
			var user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				throw new Exception("Invalid user id");
			}
			return _mapper.Map<ResponseUserInfomation>(user);
		}

		public async Task<int> UpdateCustomerInformation(RequestUpdateCustomerInformation updateCustomerInformation)
		{
			int userId = _currentUserService.GetUserId();
			var user = await _userRepository.GetUserById(userId);
			if (user == null)
			{
				throw new Exception("User does not exist");
			}
			if (user.Role.RoleName.ToLower() != "customer")
			{
				throw new Exception("The updated account is not a customer");
			}
			user.FullName = updateCustomerInformation.FullName;
			user.Phone = updateCustomerInformation.Phone;
			user.Thumbnail = updateCustomerInformation.Thumbnail;
			user.Address = updateCustomerInformation.Address;
			user.BankName = updateCustomerInformation.BankName;
			user.BankAccountNumber = updateCustomerInformation.BankAccountNumber;

			await _userRepository.UpdateAsync(user);
			return await _unitOfWork.SaveChanges();
		}

		public async Task<int> UpdateStaffInformation(RequestUpdateStaffInformation updateStaffInformation, int staffId)
		{
			//Kiểm tra người cập nhật có phải là admin không
			int userId = _currentUserService.GetUserId();
			var user = await _userRepository.GetByIdAsync(userId);
			if (user.Role.RoleName.ToLower() != "admin")
			{
				throw new Exception("You do not have permission to update account staff");
			}

			var staff = await _userRepository.GetUserById(staffId);
			if (staff == null)
			{
				throw new Exception("User does not exist");
			}
			if (staff.Role.RoleName.ToLower() != "staff")
			{
				throw new Exception("The updated account is not a staff");
			}
			staff.FullName = updateStaffInformation.FullName;
			staff.Phone = updateStaffInformation.Phone;
			staff.Thumbnail = updateStaffInformation.Thumbnail;
			staff.Address = updateStaffInformation.Address;

			await _userRepository.UpdateAsync(staff);
			return await _unitOfWork.SaveChanges();

		}

		public async Task<int> DeleteStaff(int staffId)
		{
			//Kiểm tra người xóa có phải là admin không
			int userId = _currentUserService.GetUserId();
			var user = await _userRepository.GetByIdAsync(userId);
			if (user.Role.RoleName.ToLower() != "admin")
			{
				throw new Exception("You do not have permission to delete account staff");
			}

			var staff = await _userRepository.GetUserById(staffId);
			if (staff == null)
			{
				throw new Exception("User does not exist");
			}
			if (staff.IsActive == false)
			{
				throw new Exception("Account has been deleted");
			}
			if (staff.Role.RoleName.ToLower() != "staff")
			{
				throw new Exception("The account is not a staff");
			}
			staff.IsActive = false;

			await _userRepository.UpdateAsync(staff);
			return await _unitOfWork.SaveChanges();
		}

		public async Task<int> AddNewPasswordForCustomer(RequestAddNewPassword addNewPassword)
		{
			var user = await _userRepository.GetUserByEmailAsync(addNewPassword.Email);
			if (user == null)
			{
				throw new Exception("Account does not exist");
			}
			if (user.IsActive == false)
			{
				throw new Exception("Invalid account");
			}
			if (user.Role.RoleName.ToLower() != "customer")
			{
				throw new Exception("This account cannot change password");
			}
			if (string.IsNullOrEmpty(user.ForgotPasswordToken) || DateTime.Now > user.ForgotPasswordTokenExpiry || user.ForgotPasswordToken != addNewPassword.ForgotPasswordToken)
			{
				throw new Exception("Incorrect password change request");
			}
			if (addNewPassword.NewPassword != addNewPassword.ConfirmNewPassword)
			{
				throw new Exception("Confirm password must be same as password");
			}

			user.ForgotPasswordToken = string.Empty;
			user.Password = PasswordUtils.HashPassword(addNewPassword.NewPassword);

			await _userRepository.UpdateAsync(user);
			return await _unitOfWork.SaveChanges();

		}
	}
}
