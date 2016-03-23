using StudyBox.Core.Interfaces;
using System.Linq;

namespace StudyBox.Core.Services
{
    public class ValidationService : IValidationService
    {
        public bool CheckEmail(string email)
        {
            bool isEmailValid = email.Contains("@") && !email.Any(x => char.IsWhiteSpace(x));

            return isEmailValid;
        }

        public bool CheckIfPasswordIsToShort(string password)
        {
            bool isPasswordValid = password.Count() >= 8;

            return isPasswordValid;
        }

        public bool CheckIfPasswordContainsWhitespaces(string password)
        {
            bool isPasswordValid = !password.Any(x => char.IsWhiteSpace(x));

            return isPasswordValid;
        }

        public bool CheckIfPasswordsAreEqual(string password1, string password2)
        {
            bool arePasswordsValid = string.Equals(password1, password2);

            return arePasswordsValid;
        }

        public bool CheckIfEverythingIsFilled(string email, string password1, string password2)
        {
            bool isEverythingFilled = !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password1) && !string.IsNullOrEmpty(password2);

            return isEverythingFilled;
        }

        public bool CheckIfEverythingIsFilled(string email, string password)
        {
            bool isEverythingFilled = !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password);

            return isEverythingFilled;
        }
    }
}
