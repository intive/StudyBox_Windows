using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.Messages
{
    public class DataMessageToCreateFlashcard : MessageBase
    {
        public Deck DeckInstance { get; set; }
        public Flashcard FlashcardIntance { get; set; }

        public DataMessageToCreateFlashcard(Deck deck, Flashcard flashcard)
        {
            this.DeckInstance = deck;
            this.FlashcardIntance = flashcard;
        }
    }
}
