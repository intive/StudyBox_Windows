﻿using Newtonsoft.Json;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using System;
using System.Collections.Generic;

namespace StudyBox.Core.Services
{
    public class DeserializeJsonService : IDeserializeJsonService
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
