using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;

namespace StudyBox.Core.ViewModels
{
    public class LoginViewModel : ExtendedViewModelBase
    {
        private IInternetConnectionService _internetConnectionService;
        private IValidationService _validationService;
        private RelayCommand _loginAction;
        private RelayCommand _createAccountAction;
        private string _generalErrorMessage;
        private bool _isEmailNotValid;
        private bool _isPasswordNotValid;
        private string _email;
        private string _password;
        private bool _isGeneralError;
        private bool _rememberMe = true;


        public LoginViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, IValidationService validationService) : base(navigationService)
        {
            _internetConnectionService = internetConnectionService;
            _validationService = validationService;
        }

        public RelayCommand LoginAction
        {
            get
            {
                return _loginAction ?? (_loginAction = new RelayCommand(Login));
            }
        }

        public RelayCommand CreateAccountAction
        {
            get
            {
                return _createAccountAction ?? (_createAccountAction = new RelayCommand(CreateAccount));
            }
        }

        public string GeneralErrorMessage
        {
            get { return _generalErrorMessage; }
            set
            {
                if (_generalErrorMessage != value)
                {
                    _generalErrorMessage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsEmailNotValid
        {
            get { return _isEmailNotValid; }
            set
            {
                if (_isEmailNotValid != value)
                {
                    _isEmailNotValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsPasswordNotValid
        {
            get { return _isPasswordNotValid; }
            set
            {
                if (_isPasswordNotValid != value)
                {
                    _isPasswordNotValid = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsGeneralError
        {
            get { return _isGeneralError; }
            set
            {
                if (_isGeneralError != value)
                {
                    _isGeneralError = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool RememberMe
        {
            get
            {
                return _rememberMe;
            }
            set
            {
                if (_rememberMe != value)
                {
                    _rememberMe = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void Login()
        {
            bool isInternet = _internetConnectionService.CheckConnection();

            if (isInternet)
            {
                IsEmailNotValid = !_validationService.CheckEmail(Email);
                IsPasswordNotValid = !_validationService.CheckIfPasswordIsToShort(Password) || !_validationService.CheckIfPasswordContainsWhitespaces(Password);

                IsGeneralError = !_validationService.CheckIfEverythingIsFilled(Email, Password);

                if (!IsGeneralError && !IsEmailNotValid && !IsPasswordNotValid)
                {
                    if (Email == "user@mail.pl" && Password == "12345678")
                    {
                        NavigationService.NavigateTo("DecksListView");
                    }
                    else
                    {
                        IsGeneralError = true;
                        GeneralErrorMessage = StringResources.GetString("AbsenceUserInDatabase");
                    }
                }
                else
                {
                    IsGeneralError = true;
                    GeneralErrorMessage = StringResources.GetString("FillAllFields");
                }
            }
            else
            {
                IsGeneralError = true;
                GeneralErrorMessage = StringResources.GetString("NoInternetConnection");
            }
        }

        public void CreateAccount()
        {
            NavigationService.NavigateTo("RegisterView");
        }
    }
}
