using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.Models
{
    public class Statistics
    {
        public int GoodAnswersCount { get; set; }
        public int BadAnswersCount { get;set; }
        public int CountOfDecks { get; set; }
        public int TestsCount { get; set; }

        public Statistics()
        {
            GoodAnswersCount = 0;
            BadAnswersCount = 0;
            CountOfDecks = 0;
            TestsCount = 0;
        }

        public Statistics(int goodAnswers, int badAnswers, int countOfDecks, int testsCount)
        {
            GoodAnswersCount = goodAnswers;
            BadAnswersCount = badAnswers;
            CountOfDecks = countOfDecks;
            TestsCount = testsCount;
        }
    }
}
