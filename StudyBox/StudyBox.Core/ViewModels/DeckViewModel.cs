using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.ViewModels
{
    public class DeckViewModel : ViewModelBase
    {
        private bool _isFavourite;
        private string _id;
        private DateTime _dateTime;

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                }
            }
        }

        public DateTime AddToFavouriteDecksDate
        {
            get
            {
                return _dateTime;
            }
            set
            {
                if (_dateTime != value)
                {
                    _dateTime = value;
                }
            }
        }

        public bool IsFavourite
        {
            get
            {
                return _isFavourite;
            }
            set
            {
                if (_isFavourite != value)
                {
                    _isFavourite = value;
                    RaisePropertyChanged("IsFavourite");
                }
            }
        }

        public DeckViewModel() { }
        public DeckViewModel(string id)
        {
            ID = id;
            IsFavourite = false;
        }
    }
}
