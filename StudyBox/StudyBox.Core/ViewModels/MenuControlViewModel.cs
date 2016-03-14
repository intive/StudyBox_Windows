using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;

namespace StudyBox.Core.ViewModels
{
    public class MenuControlViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private bool _isPaneOpen = false;
        private RelayCommand _openMenuCommand;

        private bool _searchButtonVisibility;
        private bool _editButtonVisibility;
        private bool _saveButtonVisibility;
        private bool _exitButtonVisibility;

        public MenuControlViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            Messenger.Default.Register<MessageToMenuControl>(this, x=> HandleMenuControlMessage(x.SearchButton,x.EditButton,x.SaveButton,x.ExitButton));
            SearchButtonVisibility = true;
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

        private void OpenMenu()
        {
            IsPaneOpen = IsPaneOpen != true;
        }

        private void HandleMenuControlMessage(bool search, bool edit, bool save, bool exit)
        {
            SearchButtonVisibility = search;
            EditButtonVisibility = edit;
            SaveButtonVisibility = save;
            ExitButtonVisibility = exit;
        }
    }
}
