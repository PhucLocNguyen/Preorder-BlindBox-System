using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PreOrderBlindBox.Data.Entities;
using PreOrderBlindBox.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Services.Utils
{
    public interface ICurrentUserService
    {
        int GetUserId();
        String getUserEmail();
        Task<User> GetCurrentAccountAsync();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            try
            {
                return int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("userID")?.Value);
            }
            catch
            {
                throw new Exception("Login Before USE!!!!");
            }
        }
        public String getUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        }

        public async Task<User?> GetCurrentAccountAsync()
        {
            int userId = GetUserId();
            var account = await _userRepository.GetByIdAsync(userId);
            return account;

            //var user = _httpContextAccessor.HttpContext.User.Identity;
            //if(user == null)
            //{
            //    throw new Exception("Account is not found in the database.");
            //}
            //return (User)user;
        }

    }
}
