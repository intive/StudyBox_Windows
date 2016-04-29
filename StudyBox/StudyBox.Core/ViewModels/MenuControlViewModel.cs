using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using StudyBox.Core.Interfaces;
using Windows.UI.Popups;

namespace StudyBox.Core.ViewModels
{
    public class MenuControlViewModel : ExtendedViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly IRestService _restservice;
        private readonly IInternetConnectionService _internetConnectionService;
        private bool _isSearchOpen = false;
        private bool _isPaneOpen = false;
        private bool _isSearchButtonEnabled = false;
        private bool _logoutButtonVisibility;
        private RelayCommand _openMenuCommand;
        private RelayCommand _showSearchPanelCommand;
        private RelayCommand _cancelSearchingCommand;
        private RelayCommand _doSearchCommand;
        private RelayCommand _logoutCommand;
        private RelayCommand _loginCommand;
        private RelayCommand _testRandomDeckCommand;
        private RelayCommand _goToDecksCommand;
        private RelayCommand _goToStatisticsCommand;
        private RelayCommand _newDeckFromFileCommand;
        private string _searchingContent;
        private string _titleBar;
        private bool _searchButtonVisibility;
        private bool _saveButtonVisibility;
        private bool _exitButtonVisibility;

        public MenuControlViewModel(INavigationService navigationService, IAccountService accountService, IRestService restservice, IInternetConnectionService internetConnectionService) : base(navigationService)
        {
            Messenger.Default.Register<MessageToMenuControl>(this, x=> HandleMenuControlMessage(x.SearchButton,x.SaveButton,x.ExitButton,x.TitleString));

            SearchButtonVisibility = true;

            _accountService = accountService;
            _internetConnectionService = internetConnectionService;
            _restservice = restservice;

            LogoutButtonVisibility = _accountService.IsUserLoggedIn();
            IsPaneOpen = false;
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

        public bool IsSearchOpen
        {
            get
            {
                return _isSearchOpen;
            }
            set
            {
                if (_isSearchOpen != value)
                {
                    _isSearchOpen = value;
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

        public bool SearchButtonVisibility
        {
            get
            {
                return _searchButtonVisibility; 
            }
            private set
            {
                if (_searchButtonVisibility != value)
                {
                    _searchButtonVisibility = value;
                    if (_searchButtonVisibility == true)
                    {
                        SaveButtonVisibility = false;
                        ExitButtonVisibility = false;
                    }
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
                        SearchButtonVisibility = false;
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
                        SearchButtonVisibility = false;
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

        public RelayCommand OpenMenuCommand
        {
            get { return _openMenuCommand ?? (_openMenuCommand = new RelayCommand(OpenMenu)); }
        }

        public RelayCommand ShowSearchPanelCommand
        {
            get { return _showSearchPanelCommand ?? (_showSearchPanelCommand = new RelayCommand(ShowSearchPanel)); }
        }

        public RelayCommand CancelSearchingCommand
        {
            get { return _cancelSearchingCommand ?? (_cancelSearchingCommand = new RelayCommand(CancelSearching)); }
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

        public RelayCommand NewDeckFromFileCommand
        {
            get { return _newDeckFromFileCommand ?? (_newDeckFromFileCommand = new RelayCommand(GoToNewDeckFromFile)); }
        }

        private async void TestRandomDeck()
        {
            if (!await _internetConnectionService.IsNetworkAvailable())
            {
                MessageDialog msg = new MessageDialog(StringResources.GetString("NoInternetConnection"));
                await msg.ShowAsync();
                return;
            }
            else if (!_internetConnectionService.IsInternetAccess())
            {
                MessageDialog msg = new MessageDialog(StringResources.GetString("AccessDenied"));
                await msg.ShowAsync();
                return;
            }

            try
            {
                var deck = await _restservice.GetRandomDeck();

                if (deck != null)
                {
                    NavigationService.NavigateTo("ExamView");
                    Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
                    Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false, deck.Name));
                }
                else
                {
                    throw new Exception(StringResources.GetString("ServerConnectionError"));
                }
            }
            catch (Exception ex)
            {
                MessageDialog msg = new MessageDialog(ex.Message);
                await msg.ShowAsync();
            }
        }

        private void GoToDecks()
        {
            NavigationService.NavigateTo("DecksListView");
            IsPaneOpen = false;
            Messenger.Default.Send<ReloadMessageToDecksList>(new ReloadMessageToDecksList(true));
        }

        private void GoToStatistics()
        {
            NavigationService.NavigateTo("StatisticsView");
            Messenger.Default.Send<ReloadMessageToStatistics>(new ReloadMessageToStatistics(true));
            IsPaneOpen = false;
        }

        private void GoToNewDeckFromFile()
        {
            NavigationService.NavigateTo("ImageImportView");
            Messenger.Default.Send<NewDeckMessageToImageImport>(new NewDeckMessageToImageImport(true));
            IsPaneOpen = false;
        }

        private void DoSearch()
        {
            Messenger.Default.Send<SearchMessageToDeckList>(new SearchMessageToDeckList(SearchingContent.Trim()));
        }

        private void CancelSearching()
        {
            if (IsSearchOpen)
            {
                IsSearchOpen = false;
                SearchButtonVisibility = true;
            }
            Messenger.Default.Send<ReloadMessageToDecksList>(new ReloadMessageToDecksList(true));
        }
        private void OpenMenu()
        {
            IsPaneOpen = IsPaneOpen != true;
        }

        private void ShowSearchPanel()
        {
            IsSearchOpen = IsSearchOpen != true;
            if (IsSearchOpen)
                ExitButtonVisibility = true;
            else
                SearchButtonVisibility = true;
        }

        private void Logout()
        {
            _accountService.LogOut();
            IsPaneOpen = false;
            LogoutButtonVisibility = false;
            NavigationService.NavigateTo("LoginView");
        }

        private void Login()
        {
            IsPaneOpen = false;
            NavigationService.NavigateTo("LoginView");
        }

        private void HandleMenuControlMessage(bool search, bool save, bool exit, string title)
        {
            SearchButtonVisibility = search;
            SaveButtonVisibility = save;
            ExitButtonVisibility = exit;
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
            IsSearchOpen = false;
        }
    }
}
