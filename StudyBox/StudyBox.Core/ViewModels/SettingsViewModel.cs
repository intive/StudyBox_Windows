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

namespace StudyBox.Core.ViewModels
{
    public class SettingsViewModel : ExtendedViewModelBase
    {
        private RelayCommand _changeGravatarCommand;
        private RelayCommand _changePasswordCommand;

        private readonly ResourceDictionary _resources = Application.Current.Resources;

        public SettingsViewModel(INavigationService navigationService) : base(navigationService)
        {
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

        private void GoToChangePassword()
        {
            NavigationService.NavigateTo("ForgottenPasswordView");
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            Messenger.Default.Send<MessageToChangePassword>(new MessageToChangePassword(true));
            
        }
    }
}
