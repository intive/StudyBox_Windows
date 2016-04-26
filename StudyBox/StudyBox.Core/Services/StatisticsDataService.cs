using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using WinRTXamlToolkit.IO.Extensions;
using Path = System.IO.Path;

namespace StudyBox.Core.Services
{
    public class StatisticsDataService : IStatisticsDataService
    {
        private ResourceDictionary _resources = Application.Current.Resources;

        public StatisticsDataService()
        {
            string path1 = ApplicationData.Current.LocalFolder.Path + "/" + _resources["TestedDecksFileName"].ToString();
            if (!File.Exists(path1))
                File.Create(path1);
            string path2 = ApplicationData.Current.LocalFolder.Path + "/" + _resources["StatisticsFileName"].ToString();
            if (!File.Exists(path2))
                File.Create(path2);

        }

        public void SaveStatistics(Statistics statistics)
        {
            try
            {
                string path = ApplicationData.Current.LocalFolder.Path + "/" + _resources["StatisticsFileName"].ToString();
                if (!File.Exists(path))
                    File.Create(path);
                using (var file = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read))
                {
                    string fileContent = String.Format("{0};{1};{2};{3}", statistics.GoodAnswersCount, statistics.AnswersCount, statistics.CountOfDecks, statistics.TestsCount);
                    byte[] toBytes = Encoding.ASCII.GetBytes(fileContent);
                    if (file.CanWrite)
                    {
                        file.Write(toBytes, 0, fileContent.Length);
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        public Statistics GetStatistics()
        {
            var statistics = new Statistics();
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                string path = ApplicationData.Current.LocalFolder.Path + "/"+_resources["StatisticsFileName"].ToString();
                if (!File.Exists(path))
                    SaveStatistics(new Statistics(0,0,0,0)); 
                string fileContent = File.ReadAllText(path);
                List<string> lines = fileContent.Split(';').ToList();
                if (lines.Count == 4)
                {
                    statistics.GoodAnswersCount = Convert.ToInt32(lines[0]);
                    statistics.AnswersCount = Convert.ToInt32(lines[1]);
                    statistics.CountOfDecks = Convert.ToInt32(lines[2]);
                    statistics.TestsCount = Convert.ToInt32(lines[3]);
                }
            }
            catch (Exception ex)
            {

            }
            return statistics;
        }

        public void IncrementGoodAnswers()
        {
            Statistics statistics = GetStatistics();
            statistics.GoodAnswersCount++;
            SaveStatistics(statistics);
        }

        public void IncrementAnswers()
        {
            Statistics statistics = GetStatistics();
            statistics.AnswersCount++;
            SaveStatistics(statistics);
        }

        public void IncrementTestsCountAnswers()
        {
            Statistics statistics = GetStatistics();
            statistics.TestsCount++;
            SaveStatistics(statistics);
        }

        public void IncrementCountOfDecks(Deck deck)
        {
            if (IsDeckAlreadyTested(deck.ID))
                return;
            else
            {
                SaveTestedDecks(deck.ID);
            }
            Statistics statistics = GetStatistics();
            statistics.CountOfDecks++;
            SaveStatistics(statistics);
        }


        public void SaveTestedDecks(string deckId)
        {
            try
            {
                string path = ApplicationData.Current.LocalFolder.Path + "/" + _resources["TestedDecksFileName"].ToString();
                if (!File.Exists(path))
                    File.Create(path);
                using (StreamWriter w = File.AppendText(path))
                {
                    string fileContent = String.Format("{0};", deckId);
                    w.Write(fileContent);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool IsDeckAlreadyTested(string deckId)
        {
            bool isTested = false;
            try
            {
                string path = ApplicationData.Current.LocalFolder.Path + "/"+ _resources["TestedDecksFileName"].ToString();
                if (!File.Exists(path))
                     SaveTestedDecks("init");
                string fileContent = File.ReadAllText(path);
                List<string> lines = fileContent.Split(';').ToList();
                lines.ForEach(line =>
                {           
                    if (line == deckId)
                        isTested = true;     
                });
            }
            catch (Exception ex)
            {

            }
            return isTested;
        }
    }
}
