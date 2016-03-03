using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Messages;
using StudyBox.Model;

namespace StudyBox.ViewModel
{
    public class DecksListViewModel : ViewModelBase
    {
    
        private ObservableCollection<Deck> _decksCollection;
        private Navigation.NavigationService _nav = new Navigation.NavigationService();

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

        public DecksListViewModel()
        {
            DecksCollection = new ObservableCollection<Deck>();
            InitializeTestsCollection();

            TapTileCommand = new RelayCommand<string>(TapTile);
        }

        #region Buttons

        public ICommand TapTileCommand { get; set; }

        private void TapTile(string id)
        {
            _nav.Navigate(typeof(View.ExamView));
            Deck deck = DecksCollection.Where(x => x.ID == id).FirstOrDefault();
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
            
        }

        #endregion


    }
}
