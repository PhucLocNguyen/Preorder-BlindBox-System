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

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IRoleRepository roleRepository, IMailService mailService,
            IConfiguration configuration, IWalletService walletService, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _mailService = mailService;
            _configuration = configuration;
            _walletService = walletService;
            _currentUserService = currentUserService;

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
                    Password = PasswordUtils.HashPassword(registerAccount.Password)
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

        public async Task<int> ForgotPassword(RequestForgotPassword forgotPassword)
        {
            int userId = _currentUserService.GetUserId();
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("Account does not exist");
            }
            if (!forgotPassword.Password.Equals(forgotPassword.ConfirmPassword))
            {
                throw new Exception("Password and confirm password must be the same");
            }

            var hasPassword = PasswordUtils.HashPassword(forgotPassword.Password);
            user.Password = hasPassword;
            await _userRepository.UpdateAsync(user);
            return await _unitOfWork.SaveChanges();

        }
    }
}
