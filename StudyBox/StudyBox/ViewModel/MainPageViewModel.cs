using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StudyBox.Navigation;

namespace StudyBox.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private Navigation.NavigationService _nav = new Navigation.NavigationService();

        public MainPageViewModel()
        {
            GoToDecksListCommand = new RelayCommand(GoToDecksList);
        }


        public ICommand GoToDecksListCommand { get; set; }

        private void GoToDecksList()
        {
            _nav.Navigate(typeof (View.DecksListView));
        }


       
    }
}
