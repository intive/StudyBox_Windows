using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;

namespace StudyBox.Core.ViewModels
{
    public class RegisterViewModel : ExtendedViewModelBase
    {
        private RelayCommand _registerAction;
        private RelayCommand _cancelAction;
        private string _generalErrorMessage;
        private bool _isEmailNotValid;
        private bool _isPasswordNotValid;
        private bool _isRepeatPasswordNotValid;
        private bool _isGeneralError;
        private string _email;
        private string _password;
        private string _repeatPassword;
        private string _passwordErrorMessage;
        private IInternetConnectionService _internetConnectionService;
        private IValidationService _validationService;

        public RegisterViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, IValidationService validationService) : base(navigationService)
        {
            _internetConnectionService = internetConnectionService;
            _validationService = validationService;
        }


        public RelayCommand RegisterAction
        {
            get
            {
                return _registerAction ?? (_registerAction = new RelayCommand(Register));
            }
        }

        public RelayCommand CancelAction
        {
            get
            {
                return _cancelAction ?? (_cancelAction = new RelayCommand(Cancel));
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

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set
            {
                if (_repeatPassword != value)
                {
                    _repeatPassword = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string PasswordErrorMessage
        {
            get { return _passwordErrorMessage; }
            set
            {
                if (_passwordErrorMessage != value)
                {
                    _passwordErrorMessage = value;
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

        public bool IsRepeatPasswordNotValid
        {
            get { return _isRepeatPasswordNotValid; }
            set
            {
                if (_isRepeatPasswordNotValid != value)
                {
                    _isRepeatPasswordNotValid = value;
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

        public void Register()
        {
            bool isInternet = _internetConnectionService.CheckConnection();
            if (isInternet)
            {
                IsGeneralError = !_validationService.CheckIfEverythingIsFilled(Email, Password, RepeatPassword);
                if (!IsGeneralError)
                {
                    _email = _email.Trim();
                    IsEmailNotValid = !_validationService.CheckEmail(Email);
                    IsPasswordNotValid = !_validationService.CheckIfPasswordIsToShort(Password);
                    if (IsPasswordNotValid)
                    {
                        PasswordErrorMessage = StringResources.GetString("PasswordTooShort");
                    }
                    else
                    {
                        IsPasswordNotValid = !_validationService.CheckIfPasswordContainsWhitespaces(Password);
                        if (IsPasswordNotValid)
                        {
                            PasswordErrorMessage = StringResources.GetString("PasswordCannotHaveWhitespaces");
                        }
                    }
                    IsRepeatPasswordNotValid = !_validationService.CheckIfPasswordsAreEqual(Password, RepeatPassword);
                }
                else
                {
                    GeneralErrorMessage = StringResources.GetString("FillAllFields");
                }
            }
            else
            {
                IsGeneralError = true;
                GeneralErrorMessage = StringResources.GetString("NoInternetConnection");
            }
        }

        public void Cancel()
        {
            NavigationService.NavigateTo("DecksListView");
        }
    }
}
