using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace StudyBox.Core.ViewModels
{
    public class ManageDeckViewModel : ExtendedViewModelBase
    {
        private ObservableCollection<Flashcard> _flashcardsCollection;
        private IRestService _restService;
        private IInternetConnectionService _internetConnectionService;
        private bool _isDataLoading = false;
        private string _selectedID = "";
        private Deck _deckInstance;

        public ManageDeckViewModel(INavigationService navigationService, IRestService restService, IInternetConnectionService internetConnectionService) : base(navigationService)
        {
            Messenger.Default.Register<DataMessageToMenageDeck>(this, x => HandleDataMessage(x.DeckInstance));
            this._restService = restService;
            this._internetConnectionService = internetConnectionService;
            FlashcardsCollection = new ObservableCollection<Flashcard>();

            TapTileCommand = new RelayCommand<string>(TapTile);
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false));
        }


        public ObservableCollection<Flashcard> FlashcardsCollection
        {
            get
            {
                return _flashcardsCollection;
            }
            set
            {
                if (_flashcardsCollection != value)
                {
                    _flashcardsCollection = value;
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

        private async void InitializeFlashcardsCollection()
        {
            if (await CheckInternetConnection())
            {
                FlashcardsCollection.Clear();
                List<Flashcard> _flashcardLists = new List<Flashcard>();
                IsDataLoading = true;
                _flashcardLists = await _restService.GetFlashcards(_deckInstance.ID);
                if (_flashcardLists != null)
                {
                    _flashcardLists.ForEach(x => FlashcardsCollection.Add(x));
                }
                IsDataLoading = false;
            }
        }

        private void HandleDataMessage(Deck deckInstance)
        {
            if (deckInstance != null)
            {
                _deckInstance = deckInstance;
                InitializeFlashcardsCollection();
            }
        }

        private async Task<bool> CheckInternetConnection()
        {
            if (!await _internetConnectionService.IsNetworkAvailable())
            {
                MessageDialog msg = new MessageDialog(StringResources.GetString("NoInternetConnection"));
                await msg.ShowAsync();
                return false;
            }
            else if (!_internetConnectionService.IsInternetAccess())
            {
                MessageDialog msg = new MessageDialog(StringResources.GetString("AccessDenied"));
                await msg.ShowAsync();
                return false;
            }
            else
                return true;
        }

        public ICommand TapTileCommand { get; set; }

        private void TapTile(string id)
        {
            _selectedID = id;
        }
    }
}
