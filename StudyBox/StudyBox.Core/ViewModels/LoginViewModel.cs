using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace StudyBox.Core.ViewModels
{
    public class LoginViewModel : ExtendedViewModelBase
    {
        private RelayCommand _loginAction;
        private RelayCommand _createAccountAction;
        private string _generalErrorMessage;
        private bool _isEmailNotValid;
        private bool _isPasswordNotValid;
        private string _email;
        private string _password;
        private bool _isGeneralError;

        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {
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

        public void Login()
        {
            IsEmailNotValid = string.IsNullOrEmpty(Email);
            IsPasswordNotValid = string.IsNullOrEmpty(Password);
            GeneralErrorMessage = StringResources.GetString("NoInternetConnection");
            IsGeneralError = true;
        }

        public void CreateAccount()
        {
            NavigationService.NavigateTo("RegisterView");
        }
    }
}
