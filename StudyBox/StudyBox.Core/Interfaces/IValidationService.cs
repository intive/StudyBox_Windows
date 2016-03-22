namespace StudyBox.Core.Interfaces
{
    public interface IValidationService
    {
        bool CheckEmail(string email);
        bool CheckIfPasswordIsToShort(string password);
        bool CheckIfPasswordContainsWhitespaces(string password);
        bool CheckIfPasswordsAreEqual(string password1, string password2);
        bool CheckIfEverythingIsFilled(string email, string password1, string password2);
    }
}
