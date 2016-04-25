using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class StatisticsViewModel : ExtendedViewModelBase
    {
        private int _goodAnswersCount;
        private int _badAnswersCount;
        private int _countOfDecks;
        private int _testsCount;
        private Statistics _statistics;
        private IInternetConnectionService _internetConnetioConnectionService;
        private IRestService _restService;

        public StatisticsViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, IRestService restService) : base(navigationService)
        {
            _internetConnetioConnectionService = internetConnectionService;
            _restService = restService;
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

        public int BadAnswersCount
        {
            get
            {
                return _badAnswersCount;

            }
            set
            {
                if (_badAnswersCount != value)
                {
                    _badAnswersCount = value;
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
                //TODO get all statistics from backend
                //mock
                _statistics=new Statistics(0,0,0,0);
                GoodAnwersCount = _statistics.GoodAnswersCount;
                BadAnswersCount = _statistics.BadAnswersCount;
                CountOfDecks = _statistics.CountOfDecks;
                TestesCount = _statistics.TestsCount;
            }
            else
            {
                
            }
        }
    }
}
