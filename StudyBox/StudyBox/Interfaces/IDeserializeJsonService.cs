using StudyBox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Interfaces
{
    interface IDeserializeJsonService
    {
        Flashcard GetFlashcardFromJson(string jsonToDeserialize);
        List<Flashcard> GetFlashcardsFromJson(string jsonToDeserialize);
        Deck GetDeckFromJson(string jsonToDeserialize);
        List<Deck> GetDecksFromJson(string jsonToDeserialize);
    }
}
