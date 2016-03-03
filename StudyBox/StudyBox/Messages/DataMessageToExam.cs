using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Model;

namespace StudyBox.Messages
{
    public class DataMessageToExam : MessageBase
    {
        public Deck DeckInstance { get; set; }

        public DataMessageToExam(Deck deck)
        {
            this.DeckInstance = deck;
        }
    }
}
