using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;
using Windows.UI.Xaml.Input;

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
        private RelayCommand _sortByResultCommand;
        private RelayCommand _sortByDateCommand;
        private RelayCommand _sortByDeckNameCommand;
        private RelayCommand _showMoreCommand;
        private RelayCommand _showLessCommand;
        private RelayCommand<object> _gotFocusCommand;
        private RelayCommand<KeyRoutedEventArgs> _detectKeyDownCommand;
        private List<TestsHistory> _localHistoryList;
        private List<TestsHistory> _globalHistoryList;
        private bool _sortResultDescending;
        private int _howMuchToShow;
        private bool _isMoreAvailable;
        private bool _isLessAvailable;

        public StatisticsViewModel(INavigationService navigationService, IInternetConnectionService internetConnectionService, IRestService restService, IStatisticsDataService statisticsSercice, IDetectKeysService detectKeysService) : base(navigationService, detectKeysService)
        {
            SortResultDescending = false;
            Messenger.Default.Register<ReloadMessageToStatistics>(this,x=>HandleReloadMessage(x.Reload));
            _internetConnetioConnectionService = internetConnectionService;
            _restService = restService;
            _statisticsService = statisticsSercice;
            _howMuchToShow = 1;
            GetStatistics();
            IsMoreAvailable = true;
        }
        public RelayCommand<KeyRoutedEventArgs> DetectKeyDownCommand
        {
            get { return _detectKeyDownCommand ?? (_detectKeyDownCommand = new RelayCommand<KeyRoutedEventArgs>(DetectKeysService.DetectKeyDown)); }
        }
        public RelayCommand<object> GotFocusCommand
        {
            get { return _gotFocusCommand ?? (_gotFocusCommand = new RelayCommand<object>(DetectKeysService.GotFocus)); }
        }
        public bool IsLessAvailable
        {
            get
            {
                return _isLessAvailable;
            }
            set
            {
                if (_isLessAvailable != value)
                {
                    _isLessAvailable = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsMoreAvailable
        {
            get
            {
                return _isMoreAvailable;
            }
            set
            {
                if (_isMoreAvailable != value)
                {
                    _isMoreAvailable = value;
                    RaisePropertyChanged();
                }
            }
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

            _globalHistoryList = _statisticsService.GetTestsHistory();
            _localHistoryList = new List<TestsHistory>();
            GetRows();
            TestsHistoryCollection=new ObservableCollection<TestsHistory>();
            _localHistoryList.ForEach(x => TestsHistoryCollection.Add(x));
        }

        private void HandleReloadMessage(bool reload)
        {
            if (reload)
            {
                _howMuchToShow = 1;
                GetStatistics();
            }
        }


        public RelayCommand SortByResultCommand
        {
            get
            {
                return _sortByResultCommand ?? (_sortByResultCommand = new RelayCommand(SortListByResult));
            }
        }

        public RelayCommand SortByDateCommand
        {
            get
            {
                return _sortByDateCommand ?? (_sortByDateCommand = new RelayCommand(SortListByDate));
            }
        }

        public RelayCommand SortByDeckNameCommand
        {
            get
            {
                return _sortByDeckNameCommand ?? (_sortByDeckNameCommand = new RelayCommand(SortListByDeckName));   
            }
        }

        public RelayCommand ShowMoreCommand
        {
            get
            {
                return _showMoreCommand ??(_showMoreCommand = new RelayCommand(ShowMore));
            }
        }

        public RelayCommand ShowLessCommand
        {
            get
            {
                return _showLessCommand ?? (_showLessCommand = new RelayCommand(ShowLess));
            }
        }

        public bool SortResultDescending
        {
            get
            {
                return _sortResultDescending;
            }
            set
            {
                if (_sortResultDescending != value)
                {
                    _sortResultDescending = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void ShowMore()
        {
            _howMuchToShow++;
            GetRows();
            RefreshCollection(_howMuchToShow);
        }

        private void ShowLess()
        {
            if(_howMuchToShow >=2)
                _howMuchToShow--;
            GetRows();
            RefreshCollection(_howMuchToShow);
        }

        private void GetRows()
        {
            if (_localHistoryList != null && _globalHistoryList != null)
            {
                int addsLimit = _howMuchToShow*5;
                int startIndex = 0;
                _localHistoryList.Clear();
                _globalHistoryList.ForEach(x =>
                {
                    if (startIndex < addsLimit)
                    {
                        _localHistoryList.Add(x);
                        startIndex++;
                    }
                });
                IsLessAvailable = _localHistoryList.Count >= 6;
                IsMoreAvailable = _localHistoryList.Count != _globalHistoryList.Count;
            }
        }

        private void SortListByResult()
        {
            SortResultDescending = !SortResultDescending;
            if (_sortResultDescending)
                _localHistoryList = _localHistoryList?.OrderBy(x => x.SortingResult).ToList();        
            else
                _localHistoryList = _localHistoryList?.OrderByDescending(x => x.SortingResult).ToList();
            RefreshCollection(_howMuchToShow);
        }

        private void SortListByDate()
        {
            SortResultDescending = !SortResultDescending;
            if (_sortResultDescending)
                _localHistoryList = _localHistoryList?.OrderBy(x => x.TestsDate).ToList();
            else
                _localHistoryList = _localHistoryList?.OrderByDescending(x => x.TestsDate).ToList();
            RefreshCollection(_howMuchToShow);
        }

        private void SortListByDeckName()
        {
            SortResultDescending = !SortResultDescending;
            if (_sortResultDescending)
                _localHistoryList = _localHistoryList?.OrderBy(x => x.DeckName).ToList();
            else
                _localHistoryList = _localHistoryList?.OrderByDescending(x => x.DeckName).ToList();
            RefreshCollection(_howMuchToShow);
        }

        private void RefreshCollection(int limitCounter=1)
        {
            if (_localHistoryList != null && TestsHistoryCollection != null)
            {
                int addsLimit = limitCounter * 5;
                int startIndex = 0;
                TestsHistoryCollection.Clear();
                _localHistoryList.ForEach(x =>
                {
                    if (startIndex < addsLimit)
                    {
                        TestsHistoryCollection.Add(x);
                        startIndex++;
                    }
                });

            }
        }
    }
}
