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
        private IFavouriteDecksService _favouriteService;
        private bool _isDataLoading = false;
        private bool _isSearchMessageVisible = false;
        private bool _isDeckSelected = false;
        private bool _isNoDecks = false;
        private string _selectedID = "";
        private bool isUser = false;
        private bool _isMyDeck = false;
        private DecksType _decksType;
        private List<Deck> _favouriteDecks;
        #endregion

        #region Constructors
        public DecksListViewModel(INavigationService navigationService, IRestService restService, IInternetConnectionService internetConnectionService, IStatisticsDataService statisticsService, IAccountService accountService, IFavouriteDecksService favouriteService) : base(navigationService)
        {
            Messenger.Default.Register<ReloadMessageToDecksList>(this, x => HandleReloadMessage(x.Reload));
            Messenger.Default.Register<SearchMessageToDeckList>(this, x => HandleSearchMessage(x.SearchingContent));
            Messenger.Default.Register<DecksTypeMessage>(this, x => HandleDecksTypeMessage(x.DecksType));
            Messenger.Default.Register<ConfirmMessageToRemove>(this, x => HandleConfirmMessage(x.IsConfirmed));
            this._restService = restService;
            this._internetConnectionService = internetConnectionService;
            _accountService = accountService;
            DecksCollection = new ObservableCollection<Deck>();
            _statisticsService = statisticsService;
            _favouriteService = favouriteService;
            _favouriteDecks = new List<Deck>();
            TapTileCommand = new RelayCommand<string>(TapTile);
            AddToFavouriteCommand = new RelayCommand<string>(AddToFavourite);
            RemoveFromFavouriteCommand = new RelayCommand<string>(RemoveFromFavourite);
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

        public bool IsNoDecks
        {
            get
            {
                return _isNoDecks;
            }
            set
            {
                if (_isNoDecks != value)
                {
                    _isNoDecks = value;
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

        public bool IsUser
        {
            get
            {
                return isUser;
            }
            set
            {
                if (isUser != value)
                {
                    isUser = value;
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
                IsUser = true;

                _favouriteDecks.Clear();
                _favouriteDecks = _favouriteService.GetFavouriteDecks();
                List<Deck> _deckLists = new List<Deck>();
                IsDataLoading = true;
                try
                {
                    if (_decksType == DecksType.PublicDecks)
                    {
                        Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false));
                        _deckLists = await _restService.GetDecks();
                    }
                    else if (_decksType == DecksType.Favourite)
                    {
                        _deckLists = await _restService.GetDecks();
                        List<Deck> _deckList2 = await _restService.GetUserDecks();
                        if (_deckList2!= null)
                            _deckLists = _deckLists.Union(_deckList2).ToList();

                        if (_deckLists != null && _favouriteDecks != null)
                        {
                            _favouriteDecks.Sort((x, y) => DateTime.Compare(y.ViewModel.AddToFavouriteDecksDate, x.ViewModel.AddToFavouriteDecksDate));
                            foreach (Deck deck in _favouriteDecks)
                            {
                                Deck favouriteDeck = _deckLists.Where(x => x.ID == deck.ID).FirstOrDefault();
                                if (favouriteDeck != null)
                                {
                                    favouriteDeck.ViewModel.IsFavourite = true;
                                    favouriteDeck.ViewModel.AddToFavouriteDecksDate = deck.ViewModel.AddToFavouriteDecksDate;
                                    DecksCollection.Add(favouriteDeck);
                                }
                            }

                        }
                    }
                    else
                    {
                        Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false));
                        _deckLists = await _restService.GetUserDecks();

                    }

                    if (_deckLists != null && _decksType != DecksType.Favourite)
                    {
                        _deckLists.Sort((x, y) => string.Compare(x.Name, y.Name));
                        _deckLists.ForEach(x => DecksCollection.Add(x));
                        CheckIfDeckIsFavourite();
                    }


                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, false, ex.Message));
                }
                finally
                {
                    if (DecksCollection != null && DecksCollection.Count == 0)
                        IsNoDecks = true;

                    IsDataLoading = false;
                }
            }
        }

        private void HandleReloadMessage(bool reload)
        {
            if (reload)
            {
                IsDeckSelected = false;
                DecksCollection.Clear();
                SearchMessageVisibility = false;
                IsNoDecks = false;
                InitializeDecksCollection();
            }
        }

        private async void HandleSearchMessage(string searchingContent)
        {
            IsNoDecks = false;
            IsDeckSelected = false;
            if (await CheckInternetConnection())
            {
                List<Deck> searchList;
                DecksCollection.Clear();
                _favouriteDecks.Clear();

                SearchMessageVisibility = false;
                IsDataLoading = true;
                try
                {
                    if (_accountService.IsUserLoggedIn())
                    {
                        searchList = await _restService.GetAllDecks(true, false, true, searchingContent);
                        IsUser = true;
                    }
                    else
                    {
                        searchList = await _restService.GetAllDecks(false, false, true, searchingContent);
                        IsUser = false;
                    }
                    if (searchList != null && searchList.Count > 0)
                    {
                        searchList.Sort((x, y) => DateTime.Compare(y.CreationDate, x.CreationDate));
                        searchList.ForEach(x => DecksCollection.Add(x));
                        if (IsUser)
                        {
                            _favouriteDecks = _favouriteService.GetFavouriteDecks();
                            CheckIfDeckIsFavourite();
                        }

                        IsDataLoading = false;
                    }
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, false, ex.Message));
                }
                finally
                {

                    IsDataLoading = false;
                    if (DecksCollection != null && DecksCollection.Count == 0)
                        SearchMessageVisibility = true;
                }
            }
        }

        private void HandleDecksTypeMessage(DecksType decksType)
        {
            IsDeckSelected = false;
            IsMyDeck = false;
            IsNoDecks = false;
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
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, true,
                    StringResources.GetString("NoInternetConnection")));
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

        private void CheckIfDeckIsFavourite()
        {
            if (_favouriteDecks != null)
            {
                foreach (Deck deck in _favouriteDecks)
                {
                    Deck favouriteDeck = DecksCollection.Where(x => x.ID == deck.ID).FirstOrDefault();
                    if (favouriteDeck != null)
                    {
                        favouriteDeck.ViewModel.IsFavourite = true;
                        favouriteDeck.ViewModel.AddToFavouriteDecksDate = deck.ViewModel.AddToFavouriteDecksDate;
                    }
                }
            }

        }
        #endregion

        #region Buttons

        public ICommand TapTileCommand { get; set; }
        public ICommand AddToFavouriteCommand { get; set; }
        public ICommand RemoveFromFavouriteCommand { get; set; }
        private RelayCommand _chooseLearning;
        private RelayCommand _chooseTest;
        private RelayCommand _chooseManageDeck;
        private RelayCommand _removeDeck;
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

        public RelayCommand RemoveDeck
        {
            get
            {
                return _removeDeck ?? (_removeDeck = new RelayCommand(ConfirmRemoveDeck));
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(BackToDeck));
            }
        }

        private void AddToFavourite(string id)
        {
            if (IsUser)
            {
                Deck deck = DecksCollection.Where(x => x.ID == id).FirstOrDefault();
                if (deck.ViewModel == null)
                    deck.ViewModel = new DeckViewModel();

                deck.ViewModel.IsFavourite = true;
                deck.ViewModel.AddToFavouriteDecksDate = DateTime.Now;

                if (_favouriteDecks == null)
                    _favouriteDecks = new List<Deck>();

                _favouriteDecks.Add(deck);
                _favouriteService.SaveFavouriteDecks(_favouriteDecks);
            }

        }

        private void RemoveFromFavourite(string id)
        {
            if (IsUser)
            {
                Deck deck = DecksCollection.Where(x => x.ID == id).FirstOrDefault();
                if (deck.ViewModel == null)
                    deck.ViewModel = new DeckViewModel();

                deck.ViewModel.IsFavourite = false;
                deck.ViewModel.AddToFavouriteDecksDate = default(DateTime);

                if (_favouriteDecks != null)
                {
                    Deck deckToRemove = _favouriteDecks.Where(x => x.ID == id).FirstOrDefault();
                    _favouriteDecks.Remove(deckToRemove);
                    _favouriteService.SaveFavouriteDecks(_favouriteDecks);
                }

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

        private async void ConfirmRemoveDeck()
        {
            if (await CheckInternetConnection())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, true, true,
                    StringResources.GetString("RemoveDeckMessage")));
            }
        }

        private async void HandleConfirmMessage(bool shouldBeRemoved)
        {
            if (shouldBeRemoved)
            {
                if (await CheckInternetConnection())
                {
                    IsDataLoading = true;
                    try
                    {
                        bool success = await _restService.RemoveDeck(_selectedID);
                        if (success)
                        {
                            DecksCollection.Remove(DecksCollection.Where(x => x.ID == _selectedID).First());
                        }
                    }
                    catch { }
                    IsDataLoading = false;
                    BackToDeck();
                }
            }
        }

        private void BackToDeck()
        {
            IsDeckSelected = false;
            IsMyDeck = false;
            _selectedID = "";
            if (_decksType == DecksType.PublicDecks)
            {
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false));
            }
            else
            {
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false));
            }
        }

        private async void TapTile(string id)
        {
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            if (await CheckInternetConnection())
            {
                _selectedID = id;
                IsDeckSelected = true;
                IsMyDeck = false;

                string name = DecksCollection.Where(x => x.ID == _selectedID).FirstOrDefault().Name;
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false, name));

                if (_decksType == DecksType.MyDecks)
                    IsMyDeck = true;
                else
                {
                    try
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
                    catch (Exception ex)
                    {
                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, false, ex.Message));
                    }
                }


            }
        }

        #endregion
    }
}
