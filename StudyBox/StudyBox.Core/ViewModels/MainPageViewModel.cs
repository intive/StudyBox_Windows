using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace StudyBox.Core.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoToDecksListCommand = new RelayCommand(GoToDecksList);
        }

        public ICommand GoToDecksListCommand { get; set; }

        private void GoToDecksList()
        {
            _navigationService.NavigateTo("DecksListView");
        }
    }
}
