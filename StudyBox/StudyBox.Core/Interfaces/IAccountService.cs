using System.Threading.Tasks;
using StudyBox.Core.Models;

namespace StudyBox.Core.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Login(User user);
        bool IsUserLoggedIn();
        //void LogOut();
        string GetUserEmail();
        string GetUserPassword();
        string GetUserName();
        string GetUserId();
    }
}