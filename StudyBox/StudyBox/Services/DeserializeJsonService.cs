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
                catch(JsonSerializationException ex)
                {
                    Debug.WriteLine("JsonSerializationException - " + ex.Message);
                }
                catch(JsonException ex)
                {
                    Debug.WriteLine("JsonException - " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Other Exception - " + ex.Message);
                }
                return null;
                
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
                catch (JsonSerializationException ex)
                {
                    Debug.WriteLine("JsonSerializationException - " + ex.Message);
                }
                catch (JsonException ex)
                {
                    Debug.WriteLine("JsonException - " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Other Exception - " + ex.Message);
                }
                return null;

            }
            else
                return null;
        }

        public static List<FlashcardSet> GetFlashCardSetFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<FlashcardSet> flashCardSetObject = JsonConvert.DeserializeObject<List<FlashcardSet>>(jsonToDeserialize);
                    return flashCardSetObject;
                }
                catch (JsonSerializationException ex)
                {
                    Debug.WriteLine("JsonSerializationException - " + ex.Message);
                }
                catch (JsonException ex)
                {
                    Debug.WriteLine("JsonException - " + ex.Message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Other Exception - " + ex.Message);
                }
                return null;

            }
            else
                return null;
        }
    }
}
