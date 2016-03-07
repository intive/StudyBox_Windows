using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StudyBox.Model;
using System.Diagnostics;
using StudyBox.Interfaces;

namespace StudyBox.Services
{
    public class DeserializeJsonService: IDeserializeJsonService
    {
        public Flashcard GetFlashcardFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    Flashcard flashcardObject = JsonConvert.DeserializeObject<Flashcard>(jsonToDeserialize);
                    return flashcardObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public List<Flashcard> GetFlashcardsFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<Flashcard> flashcardObject = JsonConvert.DeserializeObject<List<Flashcard>>(jsonToDeserialize);
                    return flashcardObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public Deck GetDeckFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    Deck deckObject = JsonConvert.DeserializeObject<Deck>(jsonToDeserialize);
                    return deckObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public List<Deck> GetDecksFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<Deck> decksObject = JsonConvert.DeserializeObject<List<Deck>>(jsonToDeserialize);
                    return decksObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }
    }
}
