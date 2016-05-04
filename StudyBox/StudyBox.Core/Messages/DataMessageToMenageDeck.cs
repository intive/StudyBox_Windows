using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Models;

namespace StudyBox.Core.Messages
{
    class DataMessageToMenageDeck : MessageBase
    {
        public Deck DeckInstance { get; set; }

        public DataMessageToMenageDeck(Deck deck)
        {
            this.DeckInstance = deck;
        }
    }
}
