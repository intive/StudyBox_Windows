using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using StudyBox.View;

namespace StudyBox.ViewModel
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
