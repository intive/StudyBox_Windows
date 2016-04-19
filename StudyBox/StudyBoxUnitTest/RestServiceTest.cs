using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Services;

namespace StudyBoxUnitTest
{
    [TestClass]
    public class RestServiceTest
    {
        private IRestService _restService;
        private IDeserializeJsonService _deserializeJsonService;

        string Execute(string query)
        {
            string url = "http://78.133.154.70:2000/decks/" + query + "/flashcards";

            return new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }

        string Execute2()
        {
            string url = "http://78.133.154.70:2000/decks/";

            return new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }

        [TestMethod]
        public async Task GetFlashCardsTest()
        {
            //RestService restService = new RestService(_deserializeJsonService);
            
            string deckId = "2c8815e6-4e16-41ce-95bf-d0039c35af2d";


            //var result = await _restService.GetFlashcards(deckId);

            var results = Execute(deckId);

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void GetDecks()
        {
            var results = Execute2();

            Assert.IsNotNull(results);
        }
    }
}