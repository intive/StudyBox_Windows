using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace StudyBox.Core.Messages
{
    public class NewDeckMessageToImageImport : MessageBase
    {
        public bool IsNewDeck { get; set; }

        public NewDeckMessageToImageImport(bool isNewDeck)
        {
            IsNewDeck = isNewDeck;
        }
    }
}
