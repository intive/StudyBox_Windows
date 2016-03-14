using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class DecksListViewModel : ViewModelBase
    {

        private ObservableCollection<Deck> _decksCollection;
        private INavigationService _navigationService;

        public ObservableCollection<Deck> DecksCollection
        {
            get { return _decksCollection; }
            set
            {
                if (_decksCollection != value)
                {
                    _decksCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void InitializeTestsCollection()
        {
            int countOfDecks = 20;
            for (int i = 0; i < countOfDecks; i++)
            {
                DecksCollection.Add(new Deck(Convert.ToString(i), Convert.ToString("TestTile " + i)));
            }
        }

        public DecksListViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            DecksCollection = new ObservableCollection<Deck>();
            InitializeTestsCollection();

            TapTileCommand = new RelayCommand<string>(TapTile);
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false, false));
        }

        #region Buttons

        public ICommand TapTileCommand { get; set; }

        private void TapTile(string id)
        {
            _navigationService.NavigateTo("ExamView");
            Deck deck = DecksCollection.Where(x => x.ID == id).FirstOrDefault();
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, true, false, false));
        }

        #endregion


    }
}
