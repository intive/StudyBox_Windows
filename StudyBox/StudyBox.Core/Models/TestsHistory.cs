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
        public string Result { get; set; }

        public int SortingResult
        {
            get
            {
                return Math.Abs(ScoredPoints - AllPoints);
            }
        }

        public TestsHistory() { }

        public TestsHistory(string testsDate, string deckName, int scoredPoints, int allPoints)
        {
            TestsDate = testsDate;
            DeckName = deckName;
            ScoredPoints = scoredPoints;
            AllPoints = allPoints;
            Result = String.Format("{0}/{1}", ScoredPoints, AllPoints);
        }

        public TestsHistory(string testsDate, string deckName, string result)
        {
            TestsDate = testsDate;
            DeckName = deckName;
            Result = result;
            string[] points = result.Split('/');
            if (points != null && points.Length == 2)
            {
                ScoredPoints = Convert.ToInt32(points[0]);
                AllPoints = Convert.ToInt32(points[1]);
            }
            
        }
    }
}
