using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using StudyBox.Core.Interfaces;

namespace StudyBox.Core.Services
{
    public class UserDataStorageService : IUserDataStorageService
    {
        public void AddUserData(string resource, string email, string password)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(resource, email, password));
        }

        public bool IsUserLoggedIn()
        {
            var vault = new PasswordVault();
            var credentials = vault.RetrieveAll();
            if (credentials.Count != 0)
            {
                return true;
            }
            return false;
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public string GetUserEmail()
        {
            var vault = new PasswordVault();
            var credentials = vault.RetrieveAll();
            return credentials[0].UserName;
        }

        public string GetUserPassword()
        {
            var vault = new PasswordVault();
            var credentials = vault.RetrieveAll();
            credentials[0].RetrievePassword();
            return credentials[0].Password;
        }
    }
}
