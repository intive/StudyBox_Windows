using StudyBox.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBox.Core.Models;

namespace StudyBox.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpService _httpService;
        private readonly IUserDataStorageService _userDataStorageService;

        public AccountService(IHttpService httpService, IUserDataStorageService userDataStorageService)
        {
            _httpService = httpService;
            _userDataStorageService = userDataStorageService;

            if (_userDataStorageService.IsUserLoggedIn())
            {
                _httpService.SetAuthenticationHeader(_userDataStorageService.GetUserEmail(), _userDataStorageService.GetUserPassword());
            }
        }

        public async Task<bool> Login(User user)
        {
            var status = await _httpService.Login(user);
            var isConnected = false;
            string email = user.Email;
            string password = user.Password;

            switch (status)
            {
                case 200:
                    _userDataStorageService.AddUserData("Uzytkownik", email, password);
                    _httpService.SetAuthenticationHeader(email, password);
                    isConnected = true;
                    break;
                case 401:
                    break;
                default:
                    break;
            }

            return isConnected;
        }
        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public bool IsUserLoggedIn()
        {
            return _userDataStorageService.IsUserLoggedIn();
        }

        public string GetUserEmail()
        {
            return _userDataStorageService.GetUserEmail();
        }

        public string GetUserPassword()
        {
            return _userDataStorageService.GetUserPassword();
        }

        public string GetUserName()
        {
            throw new NotImplementedException();
        }

        public string GetUserId()
        {
            throw new NotImplementedException();
        }
    }
}
