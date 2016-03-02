using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace StudyBox.ViewModel
{
    public class DecksListViewModel : ViewModelBase
    {
        //Collection TO TESTS!
        private ObservableCollection<int> _funnyIntsCollection = new ObservableCollection<int>();

        public DecksListViewModel()
        {
            FunnyIntsCollection=new ObservableCollection<int>();
            for (int i = 0; i < 20; i++)
            {
                FunnyIntsCollection.Add(i);
            }
        }


        public ObservableCollection<int> FunnyIntsCollection
        {
            get
            {
                return _funnyIntsCollection;
            }
            set
            {
                if (_funnyIntsCollection != value)
                {
                    _funnyIntsCollection = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
