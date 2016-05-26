using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using StudyBox.Core.Interfaces;
using Windows.UI.Popups;
using StudyBox.Core.Enums;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace StudyBox.Core.ViewModels
{
    public class MenuControlViewModel : ExtendedViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly IRestService _restservice;
        private readonly IInternetConnectionService _internetConnectionService;
        private readonly IGravatarService _gravatarService;
        private readonly ResourceDictionary _resources = Application.Current.Resources;
        private bool _searchVisibility = true;
        private int _searchOpacity = 0;
        private bool _isSearchVisible = false;
        private bool _isPaneOpen = false;
        private bool _isSearchButtonEnabled = false;
        private bool _logoutButtonVisibility;
        private bool _addFlashcardVisibility;
        private bool _saveButtonVisibility;
        private bool _exitButtonVisibility;
        private RelayCommand _openMenuCommand;
        private RelayCommand _showSearchPanelCommand;
        private RelayCommand _doSearchCommand;
        private RelayCommand _logoutCommand;
        private RelayCommand _loginCommand;
        private RelayCommand _testRandomDeckCommand;
        private RelayCommand _goToDecksCommand;
        private RelayCommand _goToStatisticsCommand;
        private RelayCommand _goToAddDeckCommand;
        private RelayCommand _goToUsersDecksCommand;
        private RelayCommand _lostFocusCommand;
        private RelayCommand _goToAboutCommand;
        private RelayCommand _settingsCommand;
        private RelayCommand _favouriteDecksCommand;
        private string _searchingContent = String.Empty;
        private string _titleBar;
        private string _email;
        private BitmapImage _gravatar;

        public MenuControlViewModel(INavigationService navigationService, IAccountService accountService, IRestService restservice, IInternetConnectionService internetConnectionService, IGravatarService gravatarService) : base(navigationService)
        {
            Messenger.Default.Register<MessageToMenuControl>(this, x=> HandleMenuControlMessage(x.SearchButton,x.SaveButton,x.TitleString));
            

            _accountService = accountService;
            _internetConnectionService = internetConnectionService;
            _restservice = restservice;
            _gravatarService = gravatarService;

            LogoutButtonVisibility = _accountService.IsUserLoggedIn();
            SearchVisibility = false;
            GetGravatar();
        }

        public string TitleBar
        {
            get
            {
                if (string.IsNullOrEmpty(_titleBar))
                    return StringResources.GetString("StudyBox");
                else
                    return _titleBar;
            }
            set
            {
                if (_titleBar != value)
                {
                    _titleBar = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SearchDecks
        {
            get
            {
                return StringResources.GetString("SearchDecks");
            }
        }

        public string SearchingContent
        {
            get
            {
                return _searchingContent;
            }
            set
            {
                if (_searchingContent != value)
                {
                    _searchingContent = value;
                    if (!string.IsNullOrEmpty(_searchingContent.Trim()))
                        SearchButtonEnabled = true;
                    else
                        SearchButtonEnabled = false;
                    RaisePropertyChanged();
                }
            }
        }

        public int SearchOpacity
        {
            get
            {
                return _searchOpacity;
            }
            set
            {
                if(_searchOpacity != value)
                {
                    _searchOpacity = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsSearchVisible
        {
            get
            {
                return _isSearchVisible;
            }
            set
            {
                if(_isSearchVisible != value)
                {
                    _isSearchVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool SearchVisibility
        {
            get
            {
                return _searchVisibility;
            }
            set
            {
                if(_searchVisibility != value)
                {
                    _searchVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsPaneOpen
        {
            get
            {
                return _isPaneOpen;
            }
            set
            {
                if (_isPaneOpen != value)
                {
                    _isPaneOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool SearchButtonEnabled
        {
            get
            {
                return _isSearchButtonEnabled;
            }
            set
            {
                if (_isSearchButtonEnabled != value)
                {
                    _isSearchButtonEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool SaveButtonVisibility
        {
            get
            {
                return _saveButtonVisibility;
            }
            private set
            {
                if (_saveButtonVisibility != value)
                {
                    _saveButtonVisibility = value;
                    if (_saveButtonVisibility == true)
                    {
                        ExitButtonVisibility = false;
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool ExitButtonVisibility
        {
            get
            {
                return _exitButtonVisibility;
            }
            private set
            {
                if (_exitButtonVisibility != value)
                {
                    _exitButtonVisibility = value;
                    if (_exitButtonVisibility == true)
                    {
                        SaveButtonVisibility = false;
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool LogoutButtonVisibility
        {
            get
            {
                return _logoutButtonVisibility;
            }
            set
            {
                if (_logoutButtonVisibility != value)
                {
                    _logoutButtonVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool AddFlashcardVisibility
        {
            get
            {
                return _addFlashcardVisibility;
            }
            set
            {
                if (_addFlashcardVisibility != value)
                {
                    _addFlashcardVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public BitmapImage Gravatar
        {
            get
            {
                return _gravatar;
            }
            set
            {
                if(_gravatar != value)
                {
                    _gravatar = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if(_email != value)
                {
                    _email = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand OpenMenuCommand
        {
            get { return _openMenuCommand ?? (_openMenuCommand = new RelayCommand(OpenMenu)); }
        }

        public RelayCommand ShowSearchPanelCommand
        {
            get { return _showSearchPanelCommand ?? (_showSearchPanelCommand = new RelayCommand(ShowSearchPanel)); }
        }

        public RelayCommand DoSearchCommand
        {
            get { return _doSearchCommand ?? (_doSearchCommand = new RelayCommand(DoSearch)); }
        }

        public RelayCommand LogoutCommand
        {
            get { return _logoutCommand ?? (_logoutCommand = new RelayCommand(Logout)); }
        }

        public RelayCommand LoginCommand
        {
            get { return _loginCommand ?? (_loginCommand = new RelayCommand(Login)); }
        }

        public RelayCommand TestRandomDeckCommand
        {
            get { return _testRandomDeckCommand ?? (_testRandomDeckCommand = new RelayCommand(TestRandomDeck)); }
        }

        public RelayCommand GoToDecksCommand
        {
            get { return _goToDecksCommand ?? (_goToDecksCommand = new RelayCommand(GoToDecks)); }
        }

        public RelayCommand GoToStatisticsCommand
        {
            get { return _goToStatisticsCommand ?? (_goToStatisticsCommand = new RelayCommand(GoToStatistics)); }
        }

        public RelayCommand GoToAddDeckCommand
        {
            get { return _goToAddDeckCommand ?? (_goToAddDeckCommand = new RelayCommand(GoToAddDeck)); }
        }

        public RelayCommand GoToUsersDecksCommand
        {
            get
            {
                return _goToUsersDecksCommand ?? (_goToUsersDecksCommand = new RelayCommand(GoToUsersDecks));
            }
        }

        public RelayCommand FavouriteDecksCommand
        {
            get
            {
                return _favouriteDecksCommand ?? (_favouriteDecksCommand = new RelayCommand(GoToFavouriteDecks));
            }
        }

        public RelayCommand LostFocusCommand
        {
            get
            {
                return _lostFocusCommand ?? (_lostFocusCommand = new RelayCommand(LostFocus));
            }
        }

        public RelayCommand GoToAboutCommand
        {
            get
            {
                return _goToAboutCommand ?? (_goToAboutCommand = new RelayCommand(GoToAbout));
            }
        }

        public RelayCommand SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand = new RelayCommand(GoToSettings));
            }
        }

        private void GoToAbout()
        {
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            NavigationService.NavigateTo("AboutView");
            HideSearchingContent();
            SearchVisibility = false;
            TitleBar = StringResources.GetString("AboutTitle");
        }

        private void GoToSettings()
        {
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            NavigationService.NavigateTo("SettingsView");
            HideSearchingContent();
            SearchVisibility = false;
            TitleBar = StringResources.GetString("StudyBox");
        }

        private void HideSearchingContent()
        {
            if(SearchOpacity == 1)
            {
                SearchOpacity = 0;
                IsSearchVisible = false;
                SearchVisibility = true;
                SearchingContent = String.Empty;
            }
            if (IsPaneOpen)
            {
                IsPaneOpen = false;
            }         
        }

        private void GoToUsersDecks()
        {
            NavigationService.NavigateTo("DecksListView");
            Messenger.Default.Send<DecksTypeMessage>(new DecksTypeMessage(DecksType.MyDecks));
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            HideSearchingContent();
            SearchVisibility = false;
            TitleBar = StringResources.GetString("StudyBox");
        }

        private void GoToFavouriteDecks()
        {
            NavigationService.NavigateTo("DecksListView");
            Messenger.Default.Send<DecksTypeMessage>(new DecksTypeMessage(DecksType.Favourite));
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            HideSearchingContent();
            SearchVisibility = false;
            TitleBar = StringResources.GetString("StudyBox");
        }

        private async void TestRandomDeck()
        {
            if (!await _internetConnectionService.IsNetworkAvailable())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, true,
                    StringResources.GetString("NoInternetConnection")));
                return;
            }
            else if (!_internetConnectionService.IsInternetAccess())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("AccessDenied")));
                return;
            }

            try
            {
                var deck = await _restservice.GetRandomDeck();

                if (deck != null)
                {
                    NavigationService.NavigateTo("ExamView");
                    Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
                    Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false, deck.Name));
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
                }
                else
                {
                    throw new Exception(StringResources.GetString("ServerConnectionError"));
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, ex.Message));
            }
            finally
            {
                HideSearchingContent();
            }
        }

        private void GoToDecks()
        {
            NavigationService.NavigateTo("DecksListView");
            Messenger.Default.Send<DecksTypeMessage>(new DecksTypeMessage(DecksType.PublicDecks));
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            HideSearchingContent();
            SearchVisibility = true;
            TitleBar = StringResources.GetString("StudyBox");
        }

        private void GoToAddDeck()
        {          
            if (_accountService.IsUserLoggedIn())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
                NavigationService.NavigateTo("CreateDeckView");
                HideSearchingContent();
                SearchVisibility = false;
                SaveButtonVisibility = false;
                ExitButtonVisibility = false;
                TitleBar = StringResources.GetString("StudyBox");
            }
            else
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, true, StringResources.GetString("LoginMessage")));
        }

        private void GoToStatistics()
        {
            NavigationService.NavigateTo("StatisticsView");
            Messenger.Default.Send<ReloadMessageToStatistics>(new ReloadMessageToStatistics(true));
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            HideSearchingContent();
            SearchVisibility = false;
            TitleBar = StringResources.GetString("StudyBox");
        }

        private void DoSearch()
        {
            if (!String.IsNullOrEmpty(SearchingContent.Trim()))
            {
                NavigationService.NavigateTo("DecksListView");
                Messenger.Default.Send<SearchMessageToDeckList>(new SearchMessageToDeckList(SearchingContent.Trim()));
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
                SearchingContent = String.Empty;
            }
        }

        private void OpenMenu()
        {
            IsPaneOpen = IsPaneOpen != true;
            SearchOpacity = 0;
            IsSearchVisible = false;
            SearchingContent = String.Empty;
        }

        private void ShowSearchPanel()
        {
            SearchOpacity = 1;
            IsSearchVisible = true;
            SearchVisibility = false;
        }

        private void LostFocus()
        {
            SearchOpacity = 0;
            IsSearchVisible = false;
            SearchVisibility = true;
        }

        private void Logout()
        {
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            _accountService.LogOut();
            LogoutButtonVisibility = false;
            NavigationService.NavigateTo("LoginView");
            HideSearchingContent();
            SearchVisibility = true;
        }

        private void Login()
        {
            Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(false));
            NavigationService.NavigateTo("LoginView");
            HideSearchingContent();
            SearchVisibility = true;
        }

        private void HandleMenuControlMessage(bool search, bool save, string title)
        {
            SearchVisibility = search;
            SaveButtonVisibility = save;
            if (String.IsNullOrEmpty(title))
            {
                TitleBar = StringResources.GetString("StudyBox");
            }
            else
            {
                TitleBar = title;
            }
            LogoutButtonVisibility = _accountService.IsUserLoggedIn();
            IsPaneOpen = false;
            if (SearchVisibility)
                HideSearchingContent();
            else
            {
                SearchOpacity = 0;
                IsSearchVisible = false;
                SearchingContent = String.Empty;
            }
            GetGravatar();
        }

        private void GetGravatar()
        {
            if (_accountService.IsUserLoggedIn())
            {
                Email = _accountService.GetUserEmail();
                Gravatar = new BitmapImage(new Uri(_gravatarService.GetUserGravatarUrl(_accountService.GetUserEmail())));
            }
            else
            {
                Gravatar = new BitmapImage(new Uri(_gravatarService.GetDefaultGravatarUrl()));
            }

            Gravatar.ImageFailed += Gravatar_ImageFailed;
            
        }

        private void Gravatar_ImageFailed(object sender, Windows.UI.Xaml.ExceptionRoutedEventArgs e)
        {
            Gravatar = new BitmapImage(new Uri(_resources["GravatarPng"].ToString()));
        }
    }
}
