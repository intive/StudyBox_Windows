using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Windows.System;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Messages;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using Windows.UI.Xaml.Input;

namespace StudyBox.Core.ViewModels
{
    public class SettingsViewModel : ExtendedViewModelBase
    {
        private RelayCommand _changeGravatarCommand;
        private RelayCommand _changePasswordCommand;
        private ITokenService _tokenService;
        private IInternetConnectionService _internetConnectionService;
        private IAccountService _accountService;

        private bool _isDataLoading = false;
        private readonly ResourceDictionary _resources = Application.Current.Resources;
        private RelayCommand<object> _gotFocusCommand;
        private RelayCommand<KeyRoutedEventArgs> _detectKeyDownCommand;
        public SettingsViewModel(INavigationService navigationService, ITokenService tokenService, IAccountService accountService, IInternetConnectionService internetConnectionService, IDetectKeysService detectKeysService) : base(navigationService, detectKeysService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
            _internetConnectionService = internetConnectionService;
        }
        public RelayCommand<KeyRoutedEventArgs> DetectKeyDownCommand
        {
            get { return _detectKeyDownCommand ?? (_detectKeyDownCommand = new RelayCommand<KeyRoutedEventArgs>(DetectKeysService.DetectKeyDown)); }
        }
        public RelayCommand<object> GotFocusCommand
        {
            get { return _gotFocusCommand ?? (_gotFocusCommand = new RelayCommand<object>(DetectKeysService.GotFocus)); }
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

        public RelayCommand ChangeGravatarCommand
        {
            get
            {
                return _changeGravatarCommand ?? (_changeGravatarCommand = new RelayCommand(GoToGravatar));
            }
        }

        public RelayCommand ChangePasswordCommand
        {
            get
            {
                return _changePasswordCommand ?? (_changePasswordCommand = new RelayCommand(GoToChangePassword));
            }
        }

        private async void GoToGravatar()
        {
            await Launcher.LaunchUriAsync(new Uri(_resources["GravatarHomePage"].ToString()));
        }

        private async void GoToChangePassword()
        {
            bool isInternet = _internetConnectionService.CheckConnection();
            if (isInternet)
            {
                IsDataLoading = true;
                ResetPassword returnToken = await _tokenService.GetToken(_accountService.GetUserEmail());
                IsDataLoading = false;
                if (returnToken != null && !string.IsNullOrWhiteSpace(returnToken.Token))
                {
                    NavigationService.NavigateTo("ForgottenPasswordView");
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
                    Messenger.Default.Send<MessageToChangePassword>(new MessageToChangePassword(true, returnToken, _accountService.GetUserEmail()));
                }
                else
                {
                    if (returnToken.Code == 400)
                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ResetPassword400")));
                    else if (returnToken.Code == 500)
                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ResetPassword500")));
                    else
                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ResetPassword400")));
                }
            }
            else
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, true,
                    StringResources.GetString("NoInternetConnection")));
            }

        }
    }
}
