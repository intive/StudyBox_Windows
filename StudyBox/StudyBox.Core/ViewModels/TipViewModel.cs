using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.ViewModels
{
    public class TipViewModel : ViewModelBase
    {
        private string _id;
        private string _essence;
        private readonly int _maxEssenceCharacters = 500;

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
                    RaisePropertyChanged("ID");
                }
            }
        }

        public string Essence
        {
            get
            {
                return _essence;
            }
            set
            {
                if (_essence != value)
                {
                    _essence = value;
                    RaisePropertyChanged("Essence");
                    RaisePropertyChanged("CurrentEssenceCharactersNumber");
                    RaisePropertyChanged("IsEssenceValid");
                }
            }
        }

        public int CurrentEssenceCharactersNumber
        {
            get
            {
                return Essence.Length;
            }
        }

        public int MaxEssenceCharacters
        {
            get
            {
                return _maxEssenceCharacters;
            }
        }

        public bool IsEssenceValid
        {
            get
            {
                if (CurrentEssenceCharactersNumber > MaxEssenceCharacters || CurrentEssenceCharactersNumber == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public TipViewModel() { }

        public TipViewModel(string id, string essence)
        {
            this.ID = id;
            this.Essence = essence;
        }
    }
}
