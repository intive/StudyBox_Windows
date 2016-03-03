using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using StudyBox.Model;

namespace StudyBox.ViewModel
{
    public class DecksListViewModel : ViewModelBase
    {

        private ObservableCollection<Deck> _decksCollection;

        public ObservableCollection<Deck> decksCollection
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
                decksCollection.Add(new Deck(Convert.ToString(i), Convert.ToString("TestTile " + i)));
            }
        }

        public DecksListViewModel()
        {
            decksCollection = new ObservableCollection<Deck>();
            InitializeTestsCollection();
        }




    }
}
