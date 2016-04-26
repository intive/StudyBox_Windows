using System;
using System.Collections.Generic;
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

        public StatisticsViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, IRestService restService, IStatisticsDataService statisticsSercice) : base(navigationService)
        {
            Messenger.Default.Register<ReloadMessageToStatistics>(this,x=>HandleReloadMessage(x.Reload));
            _internetConnetioConnectionService = internetConnectionService;
            _restService = restService;
            _statisticsService = statisticsSercice;
            GetStatistics();
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
        }

        private void HandleReloadMessage(bool reload)
        {
            if (reload)
                GetStatistics();
        }
    }
}
