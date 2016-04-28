using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.Models
{
    public class TestsHistory
    {
        public string TestsDate { get; set; }
        public string DeckName { get; set; }
        public int ScoredPoints { get; set; }
        public int AllPoints { get; set; }

        public string Result
        {
            get
            {
                return String.Format("{0}/{1}", ScoredPoints, AllPoints);
            }
        }

        public TestsHistory() { }

        public TestsHistory(string testsDate, string deckName, int scoredPoints, int allPoints)
        {
            TestsDate = testsDate;
            DeckName = deckName;
            ScoredPoints = scoredPoints;
            AllPoints = allPoints;
        }
    }
}
