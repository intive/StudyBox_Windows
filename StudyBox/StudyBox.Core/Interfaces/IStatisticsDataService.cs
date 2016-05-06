using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBox.Core.Models;

namespace StudyBox.Core.Interfaces
{
    public interface IStatisticsDataService
    {
        string UserId { get; set; }
        void SaveStatistics(Statistics statistics);
        Statistics GetStatistics();
        void IncrementGoodAnswers();
        void IncrementAnswers();
        void IncrementTestsCountAnswers();
        void IncrementCountOfDecks(Deck deck);
        void SaveTestedDecks(string deckId);
        bool IsDeckAlreadyTested(string deckId);
        void SaveTestsHistory(TestsHistory testToSave);
        List<TestsHistory> GetTestsHistory();
        void InitializeFiles();
    }
}