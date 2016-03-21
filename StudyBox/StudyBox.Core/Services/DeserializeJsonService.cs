using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;

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

        public User GetUserFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    User userObject = JsonConvert.DeserializeObject<User>(jsonToDeserialize);
                    return userObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public List<User> GetUsersFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<User> usersObject = JsonConvert.DeserializeObject<List<User>>(jsonToDeserialize);
                    return usersObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public Tip GetTipFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    Tip tipObject = JsonConvert.DeserializeObject<Tip>(jsonToDeserialize);
                    return tipObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public List<Tip> GetTipsFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<Tip> tipsObject = JsonConvert.DeserializeObject<List<Tip>>(jsonToDeserialize);
                    return tipsObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public Tag GetTagFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    Tag tagObject = JsonConvert.DeserializeObject<Tag>(jsonToDeserialize);
                    return tagObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public List<Tag> GetTagsFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<Tag> tagsObject = JsonConvert.DeserializeObject<List<Tag>>(jsonToDeserialize);
                    return tagsObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public TestResult GetTestResultFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    TestResult testResultObject = JsonConvert.DeserializeObject<TestResult>(jsonToDeserialize);
                    return testResultObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
        }

        public List<TestResult> GetTestResultsFromJson(string jsonToDeserialize)
        {
            if (jsonToDeserialize != null && jsonToDeserialize != "")
            {
                try
                {
                    List<TestResult> testResultsObject = JsonConvert.DeserializeObject<List<TestResult>>(jsonToDeserialize);
                    return testResultsObject;
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
