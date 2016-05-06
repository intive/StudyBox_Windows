using StudyBox.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StudyBox.Core.Interfaces
{
    public interface IRestService
    {

        Task<List<Flashcard>> GetFlashcards(string deckId, CancellationTokenSource cts = null);
        Task<List<Flashcard>> GetFlashcardsWithTipsCount(string deckId, CancellationTokenSource cts = null);
        Task<Flashcard> GetFlashcardById(string deckId, string flashcardId, CancellationTokenSource cts = null);
        Task<Flashcard> CreateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts = null);
        Task<bool> UpdateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts = null);
        Task<bool> RemoveFlashcard(string deckId, string flashcardId, CancellationTokenSource cts = null);

        Task<List<Deck>> GetDecks(CancellationTokenSource cts = null);
        Task<List<Deck>> GetUserDecks(CancellationTokenSource cts = null);
        Task<Deck> GetDeckById(string deckId, CancellationTokenSource cts = null);
        Task<List<Deck>> GetDecksByName(string name, CancellationTokenSource cts = null);
        Task<Deck> CreateDeck(Deck deck, CancellationTokenSource cts = null);
        Task<bool> UpdateDeck(Deck deck, CancellationTokenSource cts = null);
        Task<bool> RemoveDeck(string deckId, CancellationTokenSource cts = null);
        Task<List<Deck>> GetAllDecks(bool includeOwn, bool flashcardsCount, string name, CancellationTokenSource cts = null);
        Task<Deck> GetRandomDeck(CancellationTokenSource cts = null);

        Task<List<Tip>> GetTips(string deckId, string flashcardId, CancellationTokenSource cts = null);
        Task<Tip> GetTipById(string deckId, string flashcardId, string tipId, CancellationTokenSource cts = null);
        Task<Tip> CreateTip(Tip tip, string deckId, string flashcardId, CancellationTokenSource cts = null);
        Task<bool> UpdateTip(Tip tip, string deckId, string flashcardId, CancellationTokenSource cts = null);
        Task<bool> RemoveTip(string deckId, string flashcardId, string tipId, CancellationTokenSource cts = null);

        Task<List<Tag>> GetTags(string deckId, CancellationTokenSource cts = null);
        Task<Tag> GetTagById(string deckId, string tagId, CancellationTokenSource cts = null);
        Task<Tag> CreateTag(Tag tag, string deckId, CancellationTokenSource cts = null);
        Task<bool> UpdateTag(Tag tag, string deckId, CancellationTokenSource cts = null);
        Task<bool> RemoveTag(string deckId, string tagId, CancellationTokenSource cts = null);

        Task<List<TestResult>> GetTestResults(string deckId, CancellationTokenSource cts = null);
        Task<bool> SaveTestResults(List<TestResult> testResults, string deckId, CancellationTokenSource cts = null);

        Task<User> CreateUser(User user, CancellationTokenSource cts = null);
    }
}
