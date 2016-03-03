using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StudyBox.Model;
using System.Diagnostics;

namespace StudyBox.Services
{
    public static class DeserializeJsonService
    {
        public static Flashcard GetFlashCardFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    Flashcard flashCardObject = JsonConvert.DeserializeObject<Flashcard>(jsonToDeserialize);
                    return flashCardObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static List<Flashcard> GetFlashCardsFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<Flashcard> flashCardObject = JsonConvert.DeserializeObject<List<Flashcard>>(jsonToDeserialize);
                    return flashCardObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static Deck GetDeckFromJson(string jsonToDeserialize)
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

        public static List<Deck> GetDecksFromJson(string jsonToDeserialize)
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
