using System;
using Windows.UI.Xaml.Media.Animation;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;

namespace StudyBox.Core.ViewModels
{
    public class MenuControlViewModel : ExtendedViewModelBase
    {
        private bool _isSearchOpen = false;
        private bool _isPaneOpen = false;
        private RelayCommand _openMenuCommand;
        private RelayCommand _showSearchPanelCommand;
        private RelayCommand _cancelSearchingCommand;
        private RelayCommand _doSearchCommand;
        private string _searchingContent;
        private string _titleBar;
        private bool _searchButtonVisibility;
        private bool _editButtonVisibility;
        private bool _saveButtonVisibility;
        private bool _exitButtonVisibility;

        public MenuControlViewModel(INavigationService navigationService) : base(navigationService)
        {
            Messenger.Default.Register<MessageToMenuControl>(this, x=> HandleMenuControlMessage(x.SearchButton,x.SaveButton,x.ExitButton,x.TitleString));
            SearchButtonVisibility = true;
        }

        public string TitleBar
        {
            get { return _titleBar;}
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
                        EditButtonVisibility = false;
                        SaveButtonVisibility = false;
                        ExitButtonVisibility = false;
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool EditButtonVisibility
        {
            get
            {
                return _editButtonVisibility;
            }
            private set
            {
                if (_editButtonVisibility != value)
                {
                    _editButtonVisibility = value;
                    if (_editButtonVisibility == true)
                    {
                        SearchButtonVisibility = false;
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
                        EditButtonVisibility = false;
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
                        EditButtonVisibility = false;
                        SaveButtonVisibility = false;
                    }
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

        private void DoSearch()
        {
            
        }

        private void CancelSearching()
        {
            if (IsSearchOpen)
            {
                IsSearchOpen = false;
                SearchButtonVisibility = true;
            }
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

        private void HandleMenuControlMessage(bool search, bool save, bool exit, string title)
        {
            SearchButtonVisibility = search;
            SaveButtonVisibility = save;
            ExitButtonVisibility = exit;
            if (title != String.Empty)
            {
                StringResources.GetString("StudyBox");
            }
            else
            {
                TitleBar = title;
            }
        }
    }
}
