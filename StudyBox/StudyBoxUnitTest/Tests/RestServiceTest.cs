using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using StudyBoxUnitTest.Mocks;

namespace StudyBoxUnitTest.Tests
{
    [TestClass]
    public class RestServiceTest
    {
        private readonly RestServiceMock _restService = new RestServiceMock();

        private string RandomId()
        {

            string response = "";
            int tmp = 0;

            Random random = new Random();
            tmp = random.Next(5);

            switch (tmp)
            {
                case 1:
                    response = "8474da53-80e0-4b86-a051-cb91c67e3586";
                    break;
                case 2:
                    response = "758f8fb8-ef1d-449d-80cf-73bc61a6759a";
                    break;
                case 3:
                    response = "9ef53d93-31a4-4cd4-bd8e-34a3020465fd";
                    break;
                case 4:
                    response = "f282b5a6-599c-43b1-a7ad-c0d7004feea0";
                    break;
                case 5:
                    response = "05d40314-b3c9-424b-8ff3-9d84b09c0f52";
                    break;
            }

            return response;
        }

        [TestMethod]
        [TestCategory("FlashCard")]
        public void GetFlashcardsTest()
        {
            string deckId = RandomId();

            Task.Run(async () =>
            {
                await _restService.GetFlashcards(deckId);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("FlashCard")]
        public void GetFlashcardByIdTest()
        {
            string deckId = RandomId();
            string flashcardId = RandomId();

            Task.Run(async () =>
            {
                await _restService.GetFlashcardById(deckId, flashcardId);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("Decks")]
        public void GetAllDecksTest()
        {
            bool authorize = false;
            bool includeOwn = true;
            bool flashcardsCount = false;
            string name = "Second name";

            Task.Run(async () =>
            {
                await _restService.GetAllDecks(authorize, includeOwn, flashcardsCount, name);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("Decks")]
        public void GetRandomDeckTest()
        {
            Task.Run(async () =>
            {
                await _restService.GetRandomDeck();
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        [TestCategory("Tip")]
        public void GetTipsTest()
        {
            string deckId = RandomId();
            string flashcardId = RandomId();

            Task.Run(async () =>
            {
                await _restService.GetTips(deckId, flashcardId);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("Tip")]
        public void GetTipByIdTest()
        {
            string deckId = RandomId();
            string flashcardId = RandomId();
            string tipId = RandomId();

            Task.Run(async () =>
            {
                await _restService.GetTipById(deckId, flashcardId, tipId);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("Tag")]
        public void GetTagsTest()
        {
            string deckId = RandomId();

            Task.Run(async () =>
            {
                await _restService.GetTags(deckId);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("Tag")]
        public void GetTagById()
        {
            string deckId = RandomId();
            string tagId = RandomId();

            Task.Run(async () =>
            {
                await _restService.GetTagById(deckId, tagId);
            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        [TestCategory("TestMode")]
        public void GetTestResultsTest()
        {
            string deckId = RandomId();

            Task.Run(async () =>
            {
                await _restService.GetTestResults(deckId);
            }).GetAwaiter().GetResult();
        }

    }
}