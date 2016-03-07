using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Messages;
using StudyBox.Model;

namespace StudyBox.ViewModel
{
    public class ExamViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private Deck _deckInstance;
        private string _testTextBlock;

        public ExamViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Messenger.Default.Register<DataMessageToExam>(this, x=> HandleDataMessage(x.DeckInstance));
        }

        public string TestTextBlock
        {
            get
            {
                return _testTextBlock;
            }
            set
            {
                if (_testTextBlock != value)
                {
                    _testTextBlock = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void HandleDataMessage(Deck deckInstance)
        {
            if (deckInstance != null)
            {
                _deckInstance = deckInstance;
                TestTextBlock = _deckInstance.Name;
            }
        }
    }
}
