using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class StatisticsViewModel : ExtendedViewModelBase
    {
        private int _goodAnswersCount;
        private int _answersCount;
        private int _countOfDecks;
        private int _testsCount;
        private Statistics _statistics;
        private IInternetConnectionService _internetConnetioConnectionService;
        private IRestService _restService;
        private IStatisticsDataService _statisticsService;
        private ObservableCollection<TestsHistory> _testsHistoryCollection; 

        public StatisticsViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, IRestService restService, IStatisticsDataService statisticsSercice) : base(navigationService)
        {
            Messenger.Default.Register<ReloadMessageToStatistics>(this,x=>HandleReloadMessage(x.Reload));
            _internetConnetioConnectionService = internetConnectionService;
            _restService = restService;
            _statisticsService = statisticsSercice;
            GetStatistics();
        }

        public ObservableCollection<TestsHistory> TestsHistoryCollection
        {
            get
            {
                return _testsHistoryCollection;
            }
            set
            {
                if (_testsHistoryCollection != value)
                {
                    _testsHistoryCollection = value;
                    RaisePropertyChanged();
                }
            }
        } 

        public int GoodAnwersCount
        {
            get
            {
                return _goodAnswersCount; 
                
            }
            set
            {
                if (_goodAnswersCount != value)
                {
                    _goodAnswersCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int AnswersCount
        {
            get
            {
                return _answersCount;

            }
            set
            {
                if (_answersCount != value)
                {
                    _answersCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int CountOfDecks
        {
            get
            {
                return _countOfDecks;
            }
            set
            {
                if (_countOfDecks != value)
                {
                    _countOfDecks = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int TestesCount
        {
            get
            {
                return _testsCount;
            }
            set
            {
                if (_testsCount != value)
                {
                    _testsCount = value;
                    RaisePropertyChanged();
                }
            }
        }


        private void GetStatistics()
        {
            bool isConnection = _internetConnetioConnectionService.CheckConnection();
            if (isConnection)
            {
                _statistics = _statisticsService.GetStatistics();
                GoodAnwersCount = _statistics.GoodAnswersCount;
                AnswersCount = _statistics.AnswersCount;
                CountOfDecks = _statistics.CountOfDecks;
                TestesCount = _statistics.TestsCount;
            }
            TestsHistoryCollection = new ObservableCollection<TestsHistory>()
            {
                new TestsHistory("2012-31-24", "Geografia", 3212, 34),
                new TestsHistory("1234-23-54", "Fizyka", 1232, 233),
                new TestsHistory("2134-45-65", "Wf", 1232, 45454),
                new TestsHistory("4534-34-34", "Informatyka", 12, 2334),
                new TestsHistory("1223-21-23", "asdasd", 1232, 2334),
                new TestsHistory("4123-12-12", "Geogra123123fia", 2312, 34)
            };
        }

        private void HandleReloadMessage(bool reload)
        {
            if (reload)
                GetStatistics();
        }
    }
}
