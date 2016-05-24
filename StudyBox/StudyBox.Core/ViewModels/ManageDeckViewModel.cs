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
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false));
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

        public string ChangeDeckStatus
        {
            get
            {
                if (_deckInstance != null)
                {
                    return StringResources.GetString("ChangeDeckStatus") + " " + (_deckInstance.IsPublic ? StringResources.GetString("ToPrivate") : StringResources.GetString("ToPublic"));
                }
                else
                {
                    return StringResources.GetString("ChangeDeckStatus");
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

                try
                {
                    _flashcardLists = await _restService.GetFlashcards(_deckInstance.ID);
                    if (_flashcardLists != null)
                    {
                        _flashcardLists.ForEach(x => FlashcardsCollection.Add(x));
                    }
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("OperationFailed")));
                }
                finally
                {
                    IsDataLoading = false;
                }
            }
        }

        private void HandleDataMessage(Deck deckInstance)
        {
            if (deckInstance != null)
            {
                _deckInstance = deckInstance;
                InitializeFlashcardsCollection();
                RaisePropertyChanged("ChangeDeckStatus");
            }
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

        public ICommand TapTileCommand { get; set; }

        private RelayCommand _addNewFlashcard;

        private void TapTile(string id)
        {
            _selectedID = id;
            NavigationService.NavigateTo("CreateFlashcardView");
            Flashcard flashcard = FlashcardsCollection.Where(x => x.Id == _selectedID).FirstOrDefault();
            _selectedID = String.Empty;
            Messenger.Default.Send<DataMessageToCreateFlashcard>(new DataMessageToCreateFlashcard(_deckInstance, flashcard));
        }
        public RelayCommand AddNewFlashcard
        {
            get
            {
                return _addNewFlashcard ?? (_addNewFlashcard = new RelayCommand(GoToAddNewFlashcard));
            }
        }

        private RelayCommand _addNewFlashcardFromFile;

        public RelayCommand AddNewFlashcardFromFile
        {
            get
            {
                return _addNewFlashcardFromFile ?? (_addNewFlashcardFromFile = new RelayCommand(GoToAddNewFlashcardFromFile));
            }
        }

        private RelayCommand _changeStatus;

        public RelayCommand ChangeStatus
        {
            get
            {
                return _changeStatus ?? (_changeStatus = new RelayCommand(ChangeStatusOfDeck));
            }
        }

        private async void ChangeStatusOfDeck()
        {
            if (await CheckInternetConnection())
            {
                _deckInstance.IsPublic = !_deckInstance.IsPublic;

                try
                {
                    await _restService.UpdateDeck(_deckInstance);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("OperationFailed")));
                    _deckInstance.IsPublic = !_deckInstance.IsPublic;
                }
                
                RaisePropertyChanged("ChangeDeckStatus");
            }
        }

        private void GoToAddNewFlashcard()
        {
            NavigationService.NavigateTo("CreateFlashcardView");
            Messenger.Default.Send<DataMessageToCreateFlashcard>(new DataMessageToCreateFlashcard(_deckInstance, null));
        }

        private void GoToAddNewFlashcardFromFile()
        {
            NavigationService.NavigateTo("ImageImportView");
            Messenger.Default.Send<DataMessageToCreateFlashcard>(new DataMessageToCreateFlashcard(_deckInstance, null));
        }
    }
}
