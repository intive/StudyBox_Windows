namespace StudyBox.Core.Interfaces
{
    public interface IUserDataStorageService
    {
        void AddUserData(string resource, string email, string password);
        bool IsUserLoggedIn();
        void LogOut();
        string GetUserEmail();
        string GetUserPassword();
    }
}