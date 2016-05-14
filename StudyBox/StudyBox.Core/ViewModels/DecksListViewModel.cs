using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Enums;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class DecksListViewModel : ExtendedViewModelBase
    {
        #region fields
        private ObservableCollection<Deck> _decksCollection;
        private IRestService _restService;
        private IInternetConnectionService _internetConnectionService;
        private IStatisticsDataService _statisticsService;
        private IAccountService _accountService;
        private bool _isDataLoading = false;
        private bool _isSearchMessageVisible = false;
        private bool _isDeckSelected = false;
        private string _selectedID = "";
        private bool _isMyDeck = false;
        private DecksType _decksType;
        #endregion

        #region Constructors
        public DecksListViewModel(INavigationService navigationService, IRestService restService, IInternetConnectionService internetConnectionService, IStatisticsDataService statisticsService, IAccountService accountService) : base(navigationService)
        {
            Messenger.Default.Register<ReloadMessageToDecksList>(this, x => HandleReloadMessage(x.Reload));
            Messenger.Default.Register<SearchMessageToDeckList>(this, x => HandleSearchMessage(x.SearchingContent));
            Messenger.Default.Register<DecksTypeMessage>(this, x => HandleDecksTypeMessage(x.DecksType));
            this._restService = restService;
            this._internetConnectionService = internetConnectionService;
            _accountService = accountService;
            DecksCollection = new ObservableCollection<Deck>();
            _statisticsService = statisticsService;
            //InitializeDecksCollection();

            TapTileCommand = new RelayCommand<string>(TapTile);
        }
        #endregion

        #region Getters/Setters
        public ObservableCollection<Deck> DecksCollection
        {
            get
            {
                return _decksCollection;
            }
            set
            {
                if (_decksCollection != value)
                {
                    _decksCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsDeckSelected
        {
            get
            {
                return _isDeckSelected;
            }
            set
            {
                if (_isDeckSelected != value)
                {
                    _isDeckSelected = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsMyDeck
        {
            get
            {
                return _isMyDeck;
            }
            set
            {
                if (_isMyDeck != value)
                {
                    _isMyDeck = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsDataLoading
        {
            get
            {
                return _isDataLoading;
            }
            set
            {
                if (_isDataLoading != value)
                {
                    _isDataLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool SearchMessageVisibility
        {
            get
            {
                return _isSearchMessageVisible;
            }
            set
            {
                if (_isSearchMessageVisible != value)
                {
                    _isSearchMessageVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Methods
        private async void InitializeDecksCollection()
        {
            if (await CheckInternetConnection() && _accountService.IsUserLoggedIn())
            {
                List<Deck> _deckLists = new List<Deck>();
                IsDataLoading = true;
                if (_decksType == DecksType.PublicDecks)
                    _deckLists = await _restService.GetDecks();
                else
                    _deckLists = await _restService.GetUserDecks();
                if (_deckLists != null)
                {
                    _deckLists.Sort((x, y) => string.Compare(x.Name, y.Name));
                    _deckLists.ForEach(x => DecksCollection.Add(x));
                }
                IsDataLoading = false;
            }
        }

        private void HandleReloadMessage(bool reload)
        {
            if (reload)
            {
                IsDeckSelected = false;
                DecksCollection.Clear();
                SearchMessageVisibility = false;
                InitializeDecksCollection();
            }
        }

        private async void HandleSearchMessage(string searchingContent)
        {
            IsDeckSelected = false;
            if (await CheckInternetConnection())
            {
                DecksCollection.Clear();
                SearchMessageVisibility = false;
                IsDataLoading = true;
                List<Deck> searchList = await _restService.GetAllDecks(false, true, searchingContent);
                if (searchList != null && searchList.Count > 0)
                {
                    searchList.Sort((x, y) => DateTime.Compare(y.CreationDate, x.CreationDate));
                    searchList.ForEach(x => DecksCollection.Add(x));
                    IsDataLoading = false;
                }
                else
                {
                    IsDataLoading = false;
                    SearchMessageVisibility = true;
                }
            }
        }

        private void HandleDecksTypeMessage(DecksType decksType)
        {
            IsDeckSelected = false;
            IsMyDeck = false;
            _decksType = decksType;
            DecksCollection.Clear();
            SearchMessageVisibility = false;
            InitializeDecksCollection();
            _statisticsService.InitializeFiles();
        }

        private async Task<bool> CheckInternetConnection()
        {
            if (!await _internetConnectionService.IsNetworkAvailable())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("NoInternetConnection")));
                return false;
            }
            else if (!_internetConnectionService.IsInternetAccess())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("AccessDenied")));
                return false;
            }
            else
                return true;
        }
        #endregion


        #region Buttons

        public ICommand TapTileCommand { get; set; }
        private RelayCommand _chooseLearning;
        private RelayCommand _chooseTest;
        private RelayCommand _chooseManageDeck;
        private RelayCommand _cancel;

        public RelayCommand ChooseLearning
        {
            get
            {
                return _chooseLearning ?? (_chooseLearning = new RelayCommand(GoToLearning));
            }
        }

        public RelayCommand ChooseTest
        {
            get
            {
                return _chooseTest ?? (_chooseTest = new RelayCommand(GoToTest));
            }
        }

        public RelayCommand ChooseManageDeck
        {
            get
            {
                return _chooseManageDeck ?? (_chooseManageDeck = new RelayCommand(GoToManageDeck));
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(BackToDeck));
            }
        }

        private async void GoToLearning()
        {
            if (await CheckInternetConnection())
            {
                IsDeckSelected = false;
                NavigationService.NavigateTo("LearningView");
                Deck deck = DecksCollection.Where(x => x.ID == _selectedID).FirstOrDefault();
                _selectedID = String.Empty;

                Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false, deck.Name));
            }
        }

        private async void GoToTest()
        {
            if (await CheckInternetConnection())
            {
                IsDeckSelected = false;
                NavigationService.NavigateTo("ExamView");
                Deck deck = DecksCollection.Where(x => x.ID == _selectedID).FirstOrDefault();
                _selectedID = String.Empty;
                Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false, deck.Name));

                _statisticsService.IncrementTestsCountAnswers();
                _statisticsService.IncrementCountOfDecks(deck);
            }
        }

        private async void GoToManageDeck()
        {
            if (await CheckInternetConnection())
            {
                IsDeckSelected = false;
                IsMyDeck = false;
                NavigationService.NavigateTo("ManageDeckView");
                Deck deck = DecksCollection.Where(x => x.ID == _selectedID).FirstOrDefault();
                _selectedID = String.Empty;

                Messenger.Default.Send<DataMessageToMenageDeck>(new DataMessageToMenageDeck(deck));
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false, deck.Name));
            }

        }

        private void BackToDeck()
        {
            IsDeckSelected = false;
            IsMyDeck = false;
            _selectedID = "";
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false));
        }

        private async void TapTile(string id)
        {
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            if (await CheckInternetConnection())
            {
                _selectedID = id;
                IsDeckSelected = true;
                IsMyDeck = false;

                if (_decksType == DecksType.MyDecks)
                    IsMyDeck = true;
                else
                {
                    Deck deck = await _restService.GetDeckById(_selectedID);
                    if (deck != null)
                    {
                        if (_accountService.IsUserLoggedIn())
                        {
                            if (_accountService.GetUserEmail() == deck.CreatorEmail)
                                IsMyDeck = true;
                        }
                    }
                }

                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false));
            }
        }

        #endregion
    }
}
