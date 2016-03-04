using GalaSoft.MvvmLight.Ioc;
using StudyBox.Interfaces;
using StudyBox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace StudyBox.Services
{
    public class RestService: IRestService
    {
        private ResourceDictionary _resources = App.Current.Resources;


        #region public methods

        public async Task<List<Flashcard>> GetFlashcards(string deckId)
        {
            try
            {
                string url = String.Format(_resources["FlashcardGetAllUrl"].ToString(), deckId);
                string webPageSource = await GetWebPageSource(url, null);

                return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetFlashcardsFromJson(webPageSource);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Flashcard>> GetFlashcards(string deckId, CancellationTokenSource cts)
        {
            try
            {
                string url = String.Format(_resources["FlashcardGetAllUrl"].ToString(), deckId);
                string webPageSource = await GetWebPageSource(url, cts);

                return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetFlashcardsFromJson(webPageSource);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<Flashcard> GetFlashcardById(string deckId, string flashcardId)
        {
            try
            {
                string url = String.Format(_resources["FlashcardGetByIdUrl"].ToString(), deckId, flashcardId);
                string webPageSource = await GetWebPageSource(url, null);

                return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetFlashcardFromJson(webPageSource);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Flashcard> GetFlashcardById(string deckId, string flashcardId, CancellationTokenSource cts)
        {
            try
            {
                string url = String.Format(_resources["FlashcardGetByIdUrl"].ToString(), deckId, flashcardId);
                string webPageSource = await GetWebPageSource(url, cts);

                return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetFlashcardFromJson(webPageSource);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<Flashcard> CreateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts)
        {
            return await CreateFlashcardHelper(flashcard, deckId, cts);
        }

        public async Task<Flashcard> CreateFlashcard(Flashcard flashcard, string deckId)
        {
            return await CreateFlashcardHelper(flashcard, deckId, null);
        }


        public async Task<bool> UpdateFlashcard(Flashcard flashcard, string deckId)
        {
            string url = String.Format(_resources["FlashcardUpdateUrl"].ToString(), deckId, flashcard.Id);

            return await UpdateHelper(url,
                new { question = flashcard.Question, answer = flashcard.Answer },
                null);
        }

        public async Task<bool> UpdateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts)
        {
            string url = String.Format(_resources["FlashcardUpdateUrl"].ToString(), deckId, flashcard.Id);

            return await UpdateHelper(url,
                new { question = flashcard.Question, answer = flashcard.Answer },
                cts);
        }


        public async Task<bool> RemoveFlashcard(string deckId, string flashcardId)
        {
            string url = String.Format(_resources["FlashcardRemoveUrl"].ToString(), deckId, flashcardId);
            return await RemoveHelper(url, HttpStatusCode.OK, null);
        }

        public async Task<bool> RemoveFlashcard(string deckId, string flashcardId, CancellationTokenSource cts)
        {
            string url = String.Format(_resources["FlashcardRemoveUrl"].ToString(), deckId, flashcardId);
            return await RemoveHelper(url, HttpStatusCode.OK, cts);
        }



        public async Task<List<Deck>> GetDecks()
        {
            string url = _resources["DeckGetAllUrl"].ToString();
            string webPageSource = await GetWebPageSource(url, null);

            return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetDecksFromJson(webPageSource);
        }

        public async Task<List<Deck>> GetDecks(CancellationTokenSource cts)
        {
            string url = _resources["DeckGetAllUrl"].ToString();
            string webPageSource = await GetWebPageSource(url, cts);

            return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetDecksFromJson(webPageSource);
        }



        public async Task<Deck> GetDeckById(string deckId)
        {
            string url = String.Format(_resources["DeckGetByIdUrl"].ToString(), deckId);
            string webPageSource = await GetWebPageSource(url, null);

            return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetDeckFromJson(webPageSource);
        }

        public async Task<Deck> GetDeckById(string deckId, CancellationTokenSource cts)
        {
            string url = String.Format(_resources["DeckGetByIdUrl"].ToString(), deckId);
            string webPageSource = await GetWebPageSource(url, cts);

            return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetDeckFromJson(webPageSource);
        }


        public async Task<List<Deck>> GetDecksByName(string name)
        {
            string url = String.Format(_resources["DeckGetAllByNameUrl"].ToString(), name);
            string webPageSource = await GetWebPageSource(url, null);

            return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetDecksFromJson(webPageSource);
        }

        public async Task<List<Deck>> GetDecksByName(string name, CancellationTokenSource cts)
        {
            string url = String.Format(_resources["DeckGetAllByNameUrl"].ToString(), name);
            string webPageSource = await GetWebPageSource(url, cts);

            return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetDecksFromJson(webPageSource);
        }


        public async Task<Deck> CreateDeck(Deck deck)
        {
            return await CreateDeckHelper(deck, null);
        }

        public async Task<Deck> CreateDeck(Deck deck, CancellationTokenSource cts)
        {
            return await CreateDeckHelper(deck, cts);
        }


        public async Task<bool> UpdateDeck(Deck deck)
        {
            string url = String.Format(_resources["DeckUpdateUrl"].ToString(), deck.ID);
            return await UpdateHelper(url,
                new { id = deck.ID, name = deck.Name },
                null);
        }

        public async Task<bool> UpdateDeck(Deck deck, CancellationTokenSource cts)
        {
            string url = String.Format(_resources["DeckUpdateUrl"].ToString(), deck.ID);
            return await UpdateHelper(url,
                new { id = deck.ID, name = deck.Name },
                cts);
        }


        public async Task<bool> RemoveDeck(string deckId)
        {
            string url = String.Format(_resources["DeckRemoveUrl"].ToString(), deckId);
            return await RemoveHelper(url, HttpStatusCode.NoContent, null);
        }

        public async Task<bool> RemoveDeck(string deckId, CancellationTokenSource cts)
        {
            string url = String.Format(_resources["DeckRemoveUrl"].ToString(), deckId);
            return await RemoveHelper(url, HttpStatusCode.NoContent, cts);
        }

        #endregion


        #region private methods

        private async Task<string> DecodeResponseContent(HttpResponseMessage response)
        {
            string jsonString = "";
            byte[] byteContent = await response.Content.ReadAsByteArrayAsync();

            try
            {
                string encoding = response.Content.Headers.ContentType.CharSet;
                jsonString = Encoding.GetEncoding(encoding).GetString(byteContent);
            }
            catch
            {
                jsonString = Encoding.UTF8.GetString(byteContent);
            }
            return jsonString;
        }

        private async Task<string> GetWebPageSource(string url, CancellationTokenSource cts)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response;
                if (cts == null)
                {
                    response = await httpClient.GetAsync(url);
                }
                else
                {
                    response = await httpClient.GetAsync(url, cts.Token);
                }

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new HttpRequestException();
                }

                return await DecodeResponseContent(response);
            }
        }

        private async Task<Flashcard> CreateFlashcardHelper(Flashcard flashcard, string deckId, CancellationTokenSource cts)
        {
            string url = String.Format(_resources["FlashcardCreateUrl"].ToString(), deckId);
            try
            {
                using (var client = new HttpClient())
                {
                    var apiFlashcard = new
                    {
                        question = flashcard.Question,
                        answer = flashcard.Answer
                    };

                    HttpResponseMessage response;
                    if (cts == null)
                    {
                        response = await client.PostAsJsonAsync(url, apiFlashcard);
                    }
                    else
                    {
                        response = await client.PostAsJsonAsync(url, apiFlashcard, cts.Token);
                    }

                    if (response.StatusCode != System.Net.HttpStatusCode.Created)
                    {
                        throw new HttpRequestException();
                    }

                    string json = await DecodeResponseContent(response);
                    return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetFlashcardFromJson(json);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<Deck> CreateDeckHelper(Deck deck, CancellationTokenSource cts)
        {
            string url = _resources["DeckCreateUrl"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    var apiDeck = new
                    {
                        name = deck.Name,
                    };

                    HttpResponseMessage response;
                    if (cts == null)
                    {
                        response = await client.PostAsJsonAsync(url, apiDeck);
                    }
                    else
                    {
                        response = await client.PostAsJsonAsync(url, apiDeck, cts.Token);
                    }

                    if (response.StatusCode != System.Net.HttpStatusCode.Created)
                    {
                        throw new HttpRequestException();
                    }

                    string json = await DecodeResponseContent(response);
                    return SimpleIoc.Default.GetInstance<IDeserializeJsonService>().GetDeckFromJson(json);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return null;
            }
        }


        private async Task<bool> UpdateHelper(string url, object apiUpdateObject, CancellationTokenSource cts)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    HttpResponseMessage response;
                    if (cts == null)
                    {
                        response = await client.PutAsJsonAsync(url, apiUpdateObject);
                    }
                    else
                    {
                        response = await client.PutAsJsonAsync(url, apiUpdateObject, cts.Token);
                    }

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new HttpRequestException();
                    }

                    return true;
                }
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private async Task<bool> RemoveHelper(string url, HttpStatusCode expectedStatusCode, CancellationTokenSource cts)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    HttpResponseMessage response;
                    if (cts == null)
                    {
                        response = await client.DeleteAsync(url);
                    }
                    else
                    {
                        response = await client.DeleteAsync(url, cts.Token);
                    }

                    if (response.StatusCode != expectedStatusCode)
                    {
                        throw new HttpRequestException();
                    }

                    return true;
                }
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
