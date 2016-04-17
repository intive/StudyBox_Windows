using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Models;
using System.Collections.Generic;

namespace StudyBox.Core.Messages
{
    public class DataMessageToExam : MessageBase
    {
        public Deck DeckInstance { get; set; }
        public List<Flashcard> BadAnswerFlashcards { get; set; }

        public DataMessageToExam(Deck deck, List<Flashcard> badAnswersFlashcards = null)
        {
            this.DeckInstance = deck;
            this.BadAnswerFlashcards = badAnswersFlashcards;
        }
    }
}
