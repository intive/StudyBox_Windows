using StudyBox.Core.Models;
using System.Collections.Generic;

namespace StudyBox.Core.Interfaces
{
    public interface IDeserializeJsonService
    {
        Flashcard GetFlashcardFromJson(string jsonToDeserialize);
        List<Flashcard> GetFlashcardsFromJson(string jsonToDeserialize);
        Deck GetDeckFromJson(string jsonToDeserialize);
        List<Deck> GetDecksFromJson(string jsonToDeserialize);
    }
}
