using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.ViewModels
{
    public class ForgottenPasswordViewModel : ExtendedViewModelBase
    {
        private IInternetConnectionService _internetConnectionService;
        private ITokenService _tockenService;
        private IValidationService _validationService;
        private IAccountService _accountService;
        private IRestService _restService;

        private RelayCommand _resetPasswordAction;
        private RelayCommand _changePasswordAction;
        private RelayCommand _cancelAction;

        private bool _isDataLoading = false;
        private bool _isEmailVisible = true;
        private bool _isEmailNotValid = false;
        private bool _isPasswordNotValid;
        private bool _isRepeatPasswordNotValid;

        private string _token = String.Empty;
        private string _email;
        private string _password;
        private string _repeatPassword;
        private string _passwordErrorMessage;
        private bool _changePassword = false;

        public ForgottenPasswordViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, ITokenService tockenService, IValidationService validationService, IRestService restService, IAccountService accountService) : base(navigationService)
        {
            Messenger.Default.Register<MessageToChangePassword>(this, x => HandleChangePasswordMessage(x.ChangePassword, x.ReturnToken, x.Email));
            _internetConnectionService = internetConnectionService;
            _validationService = validationService;
            _restService = restService;
            _accountService = accountService;
            _tockenService = tockenService;

        }

        public RelayCommand ResetPasswordAction
        {
            get
            {
                return _resetPasswordAction ?? (_resetPasswordAction = new RelayCommand(ResetPassword));
            }
        }

        public RelayCommand ChangePasswordAction
        {
            get
            {
                return _changePasswordAction ?? (_changePasswordAction = new RelayCommand(ChangePassword));
            }
        }

        public RelayCommand CancelAction
        {
            get
            {
                return _cancelAction ?? (_cancelAction = new RelayCommand(Cancel));
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

        public bool IsDataLoading
        {
            get
            {
                return _isDataLoading;
            }
            set
            {
                if (_isDataLoading != value)
                {
                    _isDataLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsEmailVisible
        {
            get
            {
                return _isEmailVisible;
            }
            set
            {
                if (_isEmailVisible != value)
                {
                    _isEmailVisible = value;
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

        private async void GetToken(string email)
        {
            bool isInternet = _internetConnectionService.CheckConnection();
            if (isInternet)
            {
                email = email.Trim();
                IsEmailNotValid = !_validationService.CheckEmail(email);
                if (!IsEmailNotValid)
                {
                    IsDataLoading = true;
                    ResetPassword returnToken = await _tockenService.GetToken(Email);
                    IsDataLoading = false;

                    if (returnToken != null && !string.IsNullOrWhiteSpace(returnToken.Token))
                    {
                        _token = returnToken.Token;
                        IsEmailVisible = false;
                    }
                    else
                    {
                        if (returnToken.Code == 400)
                            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ResetPassword400")));
                        else if (returnToken.Code == 500)
                            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ResetPassword500")));
                        else
                            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ResetPassword400")));

                        if (_changePassword)
                        {
                            NavigationService.NavigateTo("SettingsView");
                            ClearInputs();
                        }

                    }
                }
            }
            else
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, true,
                    StringResources.GetString("NoInternetConnection")));
            }
        }

        public void ResetPassword()
        {
            GetToken(Email);
        }

        public async void ChangePassword()
        {
            bool isInternet = _internetConnectionService.CheckConnection();
            if (isInternet)
            {
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
                if (!IsRepeatPasswordNotValid && !IsPasswordNotValid)
                {
                    User user = new User();
                    user.Email = Email;
                    user.Password = Password;
                    IsDataLoading = true;
                    bool passwordChangeOK = await _restService.ChangePassword(user, _token);
                    IsDataLoading = false;
                    if (passwordChangeOK)
                    {
                        if (_accountService.IsUserLoggedIn())
                        {
                            _accountService.LogOut();
                            await _accountService.Login(user);
                        }

                        if (_changePassword)
                            NavigationService.NavigateTo("SettingsView");
                        else
                            NavigationService.NavigateTo("LoginView");

                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("PasswordChanged")));
                        ClearInputs();
                    }
                    else
                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ChangePasswordError")));
                }
            }
            else
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, true,
                    StringResources.GetString("NoInternetConnection")));
            }

        }

        public void Cancel()
        {
            if (_changePassword)
                NavigationService.NavigateTo("SettingsView");
            else
                NavigationService.NavigateTo("LoginView");
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            ClearInputs();
        }

        private void ClearInputs()
        {
            Email = null;
            IsEmailNotValid = false;
            Password = null;
            RepeatPassword = null;
            IsPasswordNotValid = false;
            IsEmailVisible = true;
        }

        private void HandleChangePasswordMessage(bool changePassword, ResetPassword returnToken, string email)
        {
            _changePassword = changePassword;
            if (_changePassword && returnToken!=null && !string.IsNullOrWhiteSpace(returnToken.Token) && !string.IsNullOrWhiteSpace(email))
            {
                Email = email;
                _token = returnToken.Token;
                IsEmailVisible = false;
            }

        }
    }
}
