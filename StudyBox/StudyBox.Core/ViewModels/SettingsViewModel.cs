using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Windows.System;
using Windows.UI.Xaml;

namespace StudyBox.Core.ViewModels
{
    public class SettingsViewModel : ExtendedViewModelBase
    {
        private RelayCommand _changeGravatarCommand;
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

        private async void GoToGravatar()
        {
            await Launcher.LaunchUriAsync(new Uri(_resources["GravatarHomePage"].ToString()));
        }
    }
}
