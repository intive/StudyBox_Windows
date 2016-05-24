using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;

namespace StudyBoxUnitTest.Mocks
{
    public class RestServiceMock : IRestService
    {
        //private readonly ResourceDictionary _resources = Application.Current.Resources;
        private readonly IDeserializeJsonService _deserializeJsonService;

        public RestServiceMock(IDeserializeJsonService deserializeJsonService)
        {
            _deserializeJsonService = deserializeJsonService;
        }

        public RestServiceMock()
        {
        }

        public async Task<List<Flashcard>> GetFlashcards(string deckId, CancellationTokenSource cts = null)
        {
            if (!string.IsNullOrEmpty(deckId))
            {
                string re1 = "(........)-(....)-(....)-(....)-(............)";

                Regex r1 = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m1 = r1.Match(deckId);

                string url = "http://78.133.154.70:2000/decks/" + deckId + "/flashcards";
                if (!string.IsNullOrEmpty(url) && m1.Success)
                {
                    string webPageSource = "[" +
                                               "{" +
                                                   "\"question\": \"Sample question\", " +
                                                   "\"answer\": \"Sample answer\"," +
                                                   "\"isHidden\": false," +
                                                   "\"id\": \"12345678-9012-3456-7890-123456789012\"" +
                                               "}," +
                                               "{" +
                                                   "\"question\": \"Second question\", " +
                                                   "\"answer\": \"Second answer\"," +
                                                   "\"isHidden\": true," +
                                                   "\"id\": \"12345678-9012-3456-7890-123456789012\"" +
                                               "}" +
                                           "]";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<List<Flashcard>>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                        throw exception;
                }
            }

            return null;
        }

        public Task<List<Flashcard>> GetFlashcardsWithTipsCount(string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Flashcard> GetFlashcardById(string deckId, string flashcardId, CancellationTokenSource cts = null)
        {
            if (!string.IsNullOrEmpty(deckId) && !string.IsNullOrEmpty(flashcardId))
            {
                string re1 = "(........)-(....)-(....)-(....)-(............)";

                Regex r1 = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m1 = r1.Match(deckId);
                Match m2 = r1.Match(flashcardId);

                string url = "http://78.133.154.70:2000/decks/" + deckId + "/flashcards/" + flashcardId;
                if (!string.IsNullOrEmpty(url) && m1.Success && m2.Success)
                {
                    string webPageSource = "{" +
                                               "\"question\": \"Sample question\", " +
                                               "\"answer\": \"Sample answer\"," +
                                               "\"isHidden\": false," +
                                               "\"id\": \"12345678-9012-3456-7890-123456789012\"" +
                                           "}";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<Flashcard>(webPageSource);
                        //var result = await JsonConvert.DeserializeObject<Task<Flashcard>>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                        throw exception;
                }
            }

            return null;
        }

        public Task<Flashcard> CreateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFlashcard(string deckId, string flashcardId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Deck>> GetDecks(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Deck>> GetUserDecks(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<Deck> GetDeckById(string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Deck>> GetDecksByName(string name, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<Deck> CreateDeck(Deck deck, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDeck(Deck deck, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveDeck(string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Deck>> GetAllDecks(bool authorize,  bool includeOwn, bool flashcardsCount, string name, CancellationTokenSource cts = null)
        {
            if (includeOwn && flashcardsCount && !string.IsNullOrEmpty(name))
            {
                string url = "http://78.133.154.70:2000/decks?includeOwn=" + includeOwn + "&amp;isEnabled=" + flashcardsCount + "&amp;name=" + name;
                if (!string.IsNullOrEmpty(url))
                {
                    string webPageSource = "[" +
                                               "{" +
                                                   "\"name\": \"First name\"," +
                                                   "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"isPublic\": true," +
                                                   "\"creatorEmail\": \"test@mail.com\"" +
                                               "}," +
                                               "{" +
                                                   "\"name\": \"Second name\"," +
                                                   "\"id\": \"6d48d685-53f4-4e41-aea0-ec54d8919f0b\"," +
                                                   "\"isPublic\": false," +
                                                   "\"creatorEmail\": \"test@mail.com\"" +
                                               "}," +
                                               "{" +
                                                   "\"name\": \"Third name\"," +
                                                   "\"id\": \"0d081870-553c-4df7-b9e2-be7e5ba791c0\"," +
                                                   "\"isPublic\": false," +
                                                   "\"creatorEmail\": \"test@mail.com\"" +
                                               "}" +
                                           "]";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<List<Deck>>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                        throw exception;
                }
            }

            return null;
        }

        public async Task<Deck> GetRandomDeck(CancellationTokenSource cts = null)
        {
            string url = "http://private-anon-015b2f748-studybox.apiary-mock.com/decks?random=true";
            if (!string.IsNullOrEmpty(url))
            {
                string webPageSource = "{" +
                                       "\"name\": \"First name\"," +
                                       "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                       "\"isPublic\": true," +
                                       "\"creatorEmail\": \"test@mail.com\"" +
                                       "}";

                try
                {
                    var result = _deserializeJsonService.GetObjectFromJson<Deck>(webPageSource);
                    return result;
                }
                catch (Exception)
                {

                    return null;
                }
            }
            else
            {
                Exception exception = null;
                if (true)
                    throw exception;
            }
        }

        public async Task<List<Tip>> GetTips(string deckId, string flashcardId, CancellationTokenSource cts = null)
        {
            if (!string.IsNullOrEmpty(deckId) && !string.IsNullOrEmpty(flashcardId))
            {
                string re1 = "(........)-(....)-(....)-(....)-(............)";

                Regex r1 = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m1 = r1.Match(deckId);
                Match m2 = r1.Match(flashcardId);

                string url = "http://private-anon-7dd4d836c-studybox.apiary-mock.com/decks/" + deckId + "/flashcards/" + flashcardId + "/tips";
                if (!string.IsNullOrEmpty(url) && m1.Success && m2.Success)
                {
                    string webPageSource = "[" +
                                               "{" +
                                                   "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"essence\": \"First tip content\"," +
                                                   "\"difficult\": \"0\"" +
                                               "}," +
                                               "{" +
                                                   "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"essence\": \"Second tip content\"," +
                                                   "\"difficult\": \"4\"" +
                                               "}," +
                                               "{" +
                                                   "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"essence\": \"Third tip content\"," +
                                                   "\"difficult\": \"9\"" +
                                               "}" +
                                           "]";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<List<Tip>>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                        throw exception;
                }
            }

            return null;
        }

        public async Task<Tip> GetTipById(string deckId, string flashcardId, string tipId, CancellationTokenSource cts = null)
        {
            if (!string.IsNullOrEmpty(deckId) && !string.IsNullOrEmpty(flashcardId) && !string.IsNullOrEmpty(tipId))
            {
                string re1 = "(........)-(....)-(....)-(....)-(............)";

                Regex r1 = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m1 = r1.Match(deckId);
                Match m2 = r1.Match(flashcardId);
                Match m3 = r1.Match(tipId);

                string url = "http://private-anon-7dd4d836c-studybox.apiary-mock.com/decks/" + deckId + "/flashcards/" + flashcardId + "/tips/" + tipId;
                if (!string.IsNullOrEmpty(url) && m1.Success && m2.Success && m3.Success)
                {
                    string webPageSource = "{" +
                                               "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                               "\"essence\": \"Sample content\"," +
                                               "\"difficult\": \"9\"" +
                                           "}";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<Tip>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                        throw exception;
                }
            }

            return null;
        }

        public Task<Tip> CreateTip(Tip tip, string deckId, string flashcardId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTip(Tip tip, string deckId, string flashcardId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveTip(string deckId, string flashcardId, string tipId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tag>> GetTags(string deckId, CancellationTokenSource cts = null)
        {
            if (!string.IsNullOrEmpty(deckId))
            {
                string re1 = "(........)-(....)-(....)-(....)-(............)";

                Regex r1 = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m1 = r1.Match(deckId);

                string url = "http://private-anon-7dd4d836c-studybox.apiary-mock.com/decks/" + deckId + "/tags";
                if (!string.IsNullOrEmpty(url) && m1.Success)
                {
                    string webPageSource = "[" +
                                               "{" +
                                                   "\"id\": \"2b8970c6-db84-4bbc-80d6-093f0aa22bdd\"," +
                                                   "\"name\": \"First tag\"," +
                                               "}," +
                                               "{" +
                                                   "\"id\": \"8ccc1c09-3f4a-4734-9c04-dc7bc4df15e2\"," +
                                                   "\"name\": \"Second tag\"," +
                                               "}," +
                                               "{" +
                                                   "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"name\": \"Third tag\"," +
                                               "}" +
                                           "]";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<List<Tag>>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                        throw exception;
                }
            }

            return null;
        }

        public async Task<Tag> GetTagById(string deckId, string tagId, CancellationTokenSource cts = null)
        {
            if (!string.IsNullOrEmpty(deckId) && !string.IsNullOrEmpty(tagId))
            {
                string re1 = "(........)-(....)-(....)-(....)-(............)";

                Regex r1 = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m1 = r1.Match(deckId);
                Match m2 = r1.Match(tagId);

                string url = "http://private-anon-7dd4d836c-studybox.apiary-mock.com/decks/" + deckId + "/tags/" + tagId;
                if (!string.IsNullOrEmpty(url) && m1.Success && m2.Success)
                {
                    string webPageSource = "{" +
                                               "\"id\": \"2b8970c6-db84-4bbc-80d6-093f0aa22bdd\"," +
                                               "\"name\": \"First tag\"," +
                                           "}";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<Tag>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                    {
                        //Debug.Assert(exception != null, "exception != null");
                        throw exception;
                    }
                }
            }

            return null;
        }

        public Task<Tag> CreateTag(Tag tag, string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTag(Tag tag, string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveTag(string deckId, string tagId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TestResult>> GetTestResults(string deckId, CancellationTokenSource cts = null)
        {
            if (!string.IsNullOrEmpty(deckId))
            {
                string re1 = "(........)-(....)-(....)-(....)-(............)";

                Regex r1 = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m1 = r1.Match(deckId);

                string url = "http://private-anon-7dd4d836c-studybox.apiary-mock.com/decks/" + deckId + "/results";
                if (!string.IsNullOrEmpty(url) && m1.Success)
                {
                    string webPageSource = "[" +
                                               "{" +
                                                   "\"flashcardId\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"correctAnswers\": \"1\"," +
                                                   "\"userId\": \"5560063c-a336-43bd-85bc-4d13973509b5\"" +
                                               "}," +
                                               "{" +
                                                   "\"flashcardId\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"correctAnswers\": \"5\"," +
                                                   "\"userId\": \"5560063c-a336-43bd-85bc-4d13973509b5\"" +
                                               "}," +
                                               "{" +
                                                   "\"id\": \"1d92b949-38c7-4a1a-a184-1b5fb469ad9b\"," +
                                                   "\"correctAnswers\": \"20\"," +
                                                   "\"userId\": \"5560063c-a336-43bd-85bc-4d13973509b5\"" +
                                               "}" +
                                           "]";

                    try
                    {
                        var result = _deserializeJsonService.GetObjectFromJson<List<TestResult>>(webPageSource);
                        return result;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
                else
                {
                    Exception exception = null;
                    if (true)
                    {
                        //Debug.Assert(exception != null, "exception != null");
                        throw exception;
                    }
                }
            }

            return null;
        }

        public Task<bool> SaveTestResults(List<TestResult> testResults, string deckId, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<User> CreateUser(User user, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetLoggedUser(CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResetPassword> ResetPassword(string email, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangePassword(User user, string token, CancellationTokenSource cts = null)
        {
            throw new NotImplementedException();
        }
    }
}
