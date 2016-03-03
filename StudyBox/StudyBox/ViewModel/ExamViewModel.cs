using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Messages;
using StudyBox.Model;

namespace StudyBox.ViewModel
{
    public class ExamViewModel : ViewModelBase
    {
        private Deck _deckInstance;
        private string _testTextBlock;

        public ExamViewModel()
        {
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
            _deckInstance = deckInstance;
            TestTextBlock = _deckInstance.Name;
        }
    }
}
