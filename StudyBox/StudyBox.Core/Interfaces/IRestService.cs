using StudyBox.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StudyBox.Core.Interfaces
{
    public interface IRestService
    {
        Task<List<Flashcard>> GetFlashcards(string deckId);
        Task<List<Flashcard>> GetFlashcards(string deckId, CancellationTokenSource cts);
        Task<Flashcard> GetFlashcardById(string deckId, string flashcardId);
        Task<Flashcard> GetFlashcardById(string deckId, string flashcardId, CancellationTokenSource cts);
        Task<Flashcard> CreateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts);
        Task<Flashcard> CreateFlashcard(Flashcard flashcard, string deckId);
        Task<bool> UpdateFlashcard(Flashcard flashcard, string deckId);
        Task<bool> UpdateFlashcard(Flashcard flashcard, string deckId, CancellationTokenSource cts);
        Task<bool> RemoveFlashcard(string deckId, string flashcardId);
        Task<bool> RemoveFlashcard(string deckId, string flashcardId, CancellationTokenSource cts);

        Task<List<Deck>> GetDecks();
        Task<List<Deck>> GetDecks(CancellationTokenSource cts);
        Task<Deck> GetDeckById(string deckId);
        Task<Deck> GetDeckById(string deckId, CancellationTokenSource cts);
        Task<List<Deck>> GetDecksByName(string name);
        Task<List<Deck>> GetDecksByName(string name, CancellationTokenSource cts);
        Task<Deck> CreateDeck(Deck deck);
        Task<Deck> CreateDeck(Deck deck, CancellationTokenSource cts);
        Task<bool> UpdateDeck(Deck deck);
        Task<bool> UpdateDeck(Deck deck, CancellationTokenSource cts);
        Task<bool> RemoveDeck(string deckId);
        Task<bool> RemoveDeck(string deckId, CancellationTokenSource cts);

        Task<List<Tip>> GetTips(string deckId, string flashcardId);
        Task<List<Tip>> GetTips(string deckId, string flashcardId, CancellationTokenSource cts);
        Task<Tip> GetTipById(string deckId, string flashcardId, string tipId);
        Task<Tip> GetTipById(string deckId, string flashcardId, string tipId, CancellationTokenSource cts);
        Task<Tip> CreateTip(Tip tip, string deckId, string flashcardId, CancellationTokenSource cts);
        Task<Tip> CreateTip(Tip tip, string deckId, string flashcardId);
        Task<bool> UpdateTip(Tip tip, string deckId, string flashcardId);
        Task<bool> UpdateTip(Tip tip, string deckId, string flashcardId, CancellationTokenSource cts);
        Task<bool> RemoveTip(string deckId, string flashcardId, string tipId);
        Task<bool> RemoveTip(string deckId, string flashcardId, string tipId, CancellationTokenSource cts);

        Task<List<Tag>> GetTags(string deckId);
        Task<List<Tag>> GetTags(string deckId, CancellationTokenSource cts);
        Task<Tag> GetTagById(string deckId, string tagId);
        Task<Tag> GetTagById(string deckId, string tagId, CancellationTokenSource cts);
        Task<Tag> CreateTag(Tag tag, string deckId, CancellationTokenSource cts);
        Task<Tag> CreateTag(Tag tag, string deckId);
        Task<bool> UpdateTag(Tag tag, string deckId);
        Task<bool> UpdateTag(Tag tag, string deckId, CancellationTokenSource cts);
        Task<bool> RemoveTag(string deckId, string tagId);
        Task<bool> RemoveTag(string deckId, string tagId, CancellationTokenSource cts);

        Task<List<TestResult>> GetTestResults(string deckId);
        Task<List<TestResult>> GetTestResults(string deckId, CancellationTokenSource cts);
        Task<bool> SaveTestResults(List<TestResult> testResults, string deckId);
        Task<bool> SaveTestResults(List<TestResult> testResults, string deckId, CancellationTokenSource cts);
    }
}
