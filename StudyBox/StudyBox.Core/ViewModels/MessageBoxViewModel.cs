using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using StudyBox.Core.Models;
using static StudyBox.Core.ViewModels.CreateFlashcardViewModel;

namespace StudyBox.Core.ViewModels
{
    public class MessageBoxViewModel : ExtendedViewModelBase
    {
        private bool _isVisible = false;
        private RelayCommand _login;
        private RelayCommand _closeWindow;
        private RelayCommand _remove;
        private RelayCommand _settingsCommand;
        private string _messageText;
        private bool _isLoginButton;
        private bool _isOkButton;
        private bool _isRemoveButton;
        private bool _isNoRemoveButton;
        private bool _isSettingsButton;

        public MessageBoxViewModel(INavigationService navigationService, IAccountService accountService) : base(navigationService)
        {
            Messenger.Default.Register<MessageToMessageBoxControl>(this, x => HandleMessageBoxControlMessage(x.Visibility, x.LoginButton, x.RemoveButton, x.SettingsButton, x.NoRemoveButton, x.Message));
            ClearButtonsVisibility();
        }

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsRemoveButton
        {
            get
            {
                return _isRemoveButton;
            }
            set
            {
                if (_isRemoveButton != value)
                {
                    _isRemoveButton = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsNoRemoveButton
        {
            get
            {
                return _isNoRemoveButton;
            }
            set
            {
                if (_isNoRemoveButton != value)
                {
                    _isNoRemoveButton = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsLoginButton
        {
            get
            {
                return _isLoginButton;
            }
            set
            {
                if (_isLoginButton != value)
                {
                    _isLoginButton = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsOkButton
        {
            get
            {
                return _isOkButton;
            }
            set
            {
                if (_isOkButton != value)
                {
                    _isOkButton = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsSettingsButton
        {
            get
            {
                return _isSettingsButton;
            }
            set
            {
                if (_isSettingsButton != value)
                {
                    _isSettingsButton = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                if (_messageText != value)
                {
                    _messageText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand Remove
        {
            get
            {
                return _remove ?? (_remove = new RelayCommand(GoToRemove));
            }
        }

        public RelayCommand Login
        {
            get { return _login ?? (_login = new RelayCommand(GoToLogin)); }
        }

        public RelayCommand CloseWindow
        {
            get { return _closeWindow ?? (_closeWindow = new RelayCommand(GoToCloseWindow)); }
        }

        public RelayCommand Settings
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand = new RelayCommand(GoToSettings));               
            }
        }

        private async void GoToSettings()
        {
            IsVisible = false;
            await Launcher.LaunchUriAsync(new Uri("ms-settings:network-wifi"));
        }

        private void GoToLogin()
        {
            IsVisible = false;
            NavigationService.NavigateTo("LoginView");
        }

        private void GoToCloseWindow()
        {
            IsVisible = false;
        }

        private void GoToRemove()
        {
            IsVisible = false;
            Messenger.Default.Send<ConfirmMessageToRemove>(new ConfirmMessageToRemove(true));
        }

        private void ClearButtonsVisibility()
        {
            IsLoginButton = false;
            IsOkButton = false;
            IsRemoveButton = false;
            IsNoRemoveButton = false;
            IsSettingsButton = false;
        }

        private void HandleMessageBoxControlMessage(bool visible, bool loginButton = false, bool removeButton = false, bool settingsButton = false, bool noRemoveButton = false, string message = "")
        {
            IsVisible = visible;
            
            if (IsVisible)
            {
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false));

                ClearButtonsVisibility();

                if (loginButton)
                    IsLoginButton = true;
                else if (removeButton)
                {
                    IsRemoveButton = true;
                    IsNoRemoveButton = true;
                }
                else if (settingsButton)
                {
                    IsOkButton = false;
                    //IsNoRemoveButton = false;
                    IsSettingsButton = true;
                }
                else
                {
                    IsOkButton = true;
                }
                
                MessageText = message;
            }
        }
    }
}
