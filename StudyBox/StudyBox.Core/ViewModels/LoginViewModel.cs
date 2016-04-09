using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class LoginViewModel : ExtendedViewModelBase
    {
        private IInternetConnectionService _internetConnectionService;
        private IValidationService _validationService;
        private readonly IAccountService _accountService;
        private RelayCommand _loginAction;
        private RelayCommand _createAccountAction;
        private string _generalErrorMessage;
        private bool _isEmailNotValid;
        private bool _isPasswordNotValid;
        private string _email;
        private string _password;
        private bool _isGeneralError;

        public LoginViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, IValidationService validationService, IAccountService accountService) : base(navigationService)
        {
            _internetConnectionService = internetConnectionService;
            _validationService = validationService;
            _accountService = accountService;
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

        public async void Login()
        {
            bool isInternet = _internetConnectionService.CheckConnection();

            if (isInternet)
            {
                try
                {
                    IsEmailNotValid = !_validationService.CheckEmail(Email);
                    IsPasswordNotValid = string.IsNullOrEmpty(Password) || !_validationService.CheckIfPasswordIsToShort(Password) || !_validationService.CheckIfPasswordContainsWhitespaces(Password);
                    IsGeneralError = !_validationService.CheckIfEverythingIsFilled(Email, Password);
                }
                catch (NullReferenceException)
                {
                    IsGeneralError = true;
                    GeneralErrorMessage = StringResources.GetString("FillAllFields");
                }

                if (!IsGeneralError && !IsEmailNotValid && !IsPasswordNotValid)
                {
                    try
                    {
                        User user = new User
                        {
                            Email = Email,
                            Password = Password
                        };

                        bool isLogged = await _accountService.Login(user);

                        if (isLogged)
                        {
                            NavigationService.NavigateTo("DecksListView");
                        }
                        else
                        {
                            IsGeneralError = true;
                            GeneralErrorMessage = StringResources.GetString("AbsenceUserInDatabase");
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
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
