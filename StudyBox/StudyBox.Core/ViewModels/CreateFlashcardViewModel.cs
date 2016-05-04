using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBox.Core.Models;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Windows.UI.Popups;

namespace StudyBox.Core.ViewModels
{


    public class CreateFlashcardViewModel : ExtendedViewModelBase
    {
        private enum Mode { EditFlashcard, CreateFlashcardAndDeck, AddNewFlashcardToDeck, None }

        private Mode _mode = Mode.None;
        private Deck _deck;
        private Flashcard _flashcard;
        private IRestService _restService;
        private IInternetConnectionService _internetConnectionService;

        private string _question;
        private bool _isPublic;
        private string _answer;
        private string _deckName;
        private string _headerMessage;
        private string _submitFormMessage;
        private bool _showDeckName;
        private bool _isGeneralError;
        private ObservableCollection<TipViewModel> _tipsCollection;

        private readonly int _maxQuestionCharacters = 1000;
        private readonly int _maxAnswerCharacters = 1000;
        private readonly int _maxDeckNameCharacters = 50;

        private RelayCommand _addTip;
        private RelayCommand _submitForm;

        public CreateFlashcardViewModel(INavigationService navigationService, IRestService restService, IInternetConnectionService internetConnectionService, IStatisticsDataService statisticsService) : base(navigationService)
        {
            Question = "";
            Answer = "";
            DeckName = "";
            this._restService = restService;
            this._internetConnectionService = internetConnectionService;
            TipsCollection = new ObservableCollection<TipViewModel>();
            Remove = new RelayCommand<string>(RemoveTip);
            Messenger.Default.Register<DataMessageToCreateFlashcard>(this, x => HandleDataMessage(x.DeckInstance, x.FlashcardIntance));
        }

        public RelayCommand AddTip
        {
            get
            {
                return _addTip ?? (_addTip = new RelayCommand(AddNewTip));
            }
        }

        public RelayCommand SubmitForm
        {
            get
            {
                return _submitForm ?? (_submitForm = new RelayCommand(CreateOrEditFlashcard));
            }
        }


        public ICommand Remove { get; set; }


        public ObservableCollection<TipViewModel> TipsCollection
        {
            get
            {
                return _tipsCollection;
            }
            set
            {
                if (_tipsCollection != value)
                {
                    _tipsCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShowDeckName
        {
            get
            {
                return _showDeckName;
            }
            set
            {
                if (_showDeckName != value)
                {
                    _showDeckName = value;
                    RaisePropertyChanged("ShowDeckName");
                }
            }
        }

        public bool IsGeneralError
        {
            get
            {
                return _isGeneralError;
            }
            set
            {
                if (_isGeneralError != value)
                {
                    _isGeneralError = value;
                    RaisePropertyChanged("IsGeneralError");
                }
            }
        }

        public string Question
        {
            get
            {
                return _question;
            }
            set
            {
                if (_question != value)
                {
                    _question = value;
                    RaisePropertyChanged("Question");
                    RaisePropertyChanged("CurrentQuestionCharactersNumber");
                }
            }
        }

        public string Answer
        {
            get
            {
                return _answer;
            }
            set
            {
                if (_answer != value)
                {
                    _answer = value;
                    RaisePropertyChanged("Answer");
                    RaisePropertyChanged("CurrentAnswerCharactersNumber");
                }
            }
        }

        public bool IsPublic
        {
            get
            {
                return _isPublic;
            }
            set
            {
                if (_isPublic != value)
                {
                    _isPublic = value;
                    RaisePropertyChanged("IsPublic");
                }
            }
        }

        public string DeckName
        {
            get
            {
                return _deckName;
            }
            set
            {
                if (_deckName != value)
                {
                    _deckName = value;
                    RaisePropertyChanged("DeckName");
                    RaisePropertyChanged("CurrentDeckNameCharactersNumber");
                }
            }
        }

        public int CurrentQuestionCharactersNumber
        {
            get
            {
                return Question.Length > _maxQuestionCharacters ? _maxAnswerCharacters : Question.Length;
            }
        }

        public int CurrentAnswerCharactersNumber
        {
            get
            {
                return Answer.Length > _maxAnswerCharacters ? _maxAnswerCharacters : Answer.Length;
            }
        }

        public int CurrentDeckNameCharactersNumber
        {
            get
            {
                return DeckName.Length > _maxDeckNameCharacters ? _maxDeckNameCharacters : DeckName.Length;
            }
        }

        public int MaxQuestionCharacters
        {
            get
            {
                return _maxQuestionCharacters;
            }
        }

        public int MaxAnswerCharacters
        {
            get
            {
                return _maxAnswerCharacters;
            }
        }

        public int MaxDeckNameCharacters
        {
            get
            {
                return _maxDeckNameCharacters;
            }
        }

        public string HeaderMessage
        {
            get
            {
                return _headerMessage;
            }
            set
            {
                if (_headerMessage != value)
                {
                    _headerMessage = value;
                    RaisePropertyChanged("HeaderMessage");
                }
            }
        }

        public string SubmitFormMessage
        {
            get
            {
                return _submitFormMessage;
            }
            set
            {
                if (_submitFormMessage != value)
                {
                    _submitFormMessage = value;
                    RaisePropertyChanged("SubmitFormMessage");
                }
            }
        }

        public void RemoveTip(string id)
        {
            TipViewModel tip = TipsCollection.Where(x => x.ID == id).FirstOrDefault();
            if (tip != null)
            {
                TipsCollection.Remove(tip);
            }
        }

        public void AddNewTip()
        {
            TipsCollection.Add(new TipViewModel(System.Guid.NewGuid().ToString(), ""));
        }

        public async void CreateOrEditFlashcard()
        {
            if (!await _internetConnectionService.IsNetworkAvailable())
            {
                ShowErrorMessage(StringResources.GetString("NoInternetConnection"));
                return;
            }
            else if (!_internetConnectionService.IsInternetAccess())
            {
                ShowErrorMessage(StringResources.GetString("AccessDenied"));
                return;
            }
            else
            {
                if (!ValidateForm())
                {
                    return;
                }

                _flashcard.Answer = Answer.TrimEnd();
                _flashcard.Question = Question.TrimEnd();

                List<Tip> tips = new List<Tip>();
                foreach (TipViewModel tip in TipsCollection)
                {
                    tips.Add(new Tip(tip.ID, tip.Prompt.TrimEnd()));
                }

                try
                {
                    switch (_mode)
                    {
                        case Mode.AddNewFlashcardToDeck:
                            Flashcard createdFlashcard = await _restService.CreateFlashcard(_flashcard, _deck.ID);

                            foreach (var tip in tips)
                            {
                                await _restService.CreateTip(tip, _deck.ID, createdFlashcard.Id);
                            }

                            break;

                        case Mode.CreateFlashcardAndDeck:
                            Deck createdDeck = await _restService.CreateDeck(new Deck("1", DeckName, IsPublic));
                            Flashcard addedFlashcard = await _restService.CreateFlashcard(_flashcard, createdDeck.ID);

                            foreach (var tip in tips)
                            {
                                await _restService.CreateTip(tip, _deck.ID, addedFlashcard.Id);
                            }

                            break;

                        case Mode.EditFlashcard:
                            string flashcardId = _flashcard.Id;
                            string deckId = _flashcard.DeckID;

                            bool result = await _restService.UpdateFlashcard(_flashcard, deckId);

                            var oldTips = await _restService.GetTips(deckId, flashcardId);

                            if (oldTips!=null)
                            {
                                foreach (var oldTip in oldTips)
                                {
                                    await _restService.RemoveTip(deckId, flashcardId, oldTip.ID);
                                }
                            }

                            foreach (var tip in tips)
                            {
                                await _restService.CreateTip(tip, deckId, flashcardId);
                            }

                            break;
                    }
                }
                catch
                {
                    ShowErrorMessage(StringResources.GetString("OperationFailed"));
                }

                switch (_mode)
                {
                    case Mode.CreateFlashcardAndDeck:
                        NavigationService.NavigateTo("DecksListView");
                        Messenger.Default.Send<ReloadMessageToDecksList>(new ReloadMessageToDecksList(true));
                        Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false));
                        break;

                    default:
                        NavigationService.NavigateTo("ManageDeckView");
                        Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false));
                        Messenger.Default.Send<DataMessageToMenageDeck>(new DataMessageToMenageDeck(_deck));
                        break;

                }

                
            }
        }

        private bool ValidateForm()
        {
            if (Answer.Trim().Length == 0 || Question.Length == 0)
            {
                IsGeneralError = true;
                return false;
            }

            if (_mode == Mode.CreateFlashcardAndDeck)
            {
                if (DeckName.Trim().Length == 0)
                {
                    IsGeneralError = true;
                    return false;
                }
            }

            foreach (var tip in TipsCollection)
            {
                if (tip.Prompt.Trim().Length == 0)
                {
                    IsGeneralError = true;
                    return false;
                }
            }

            IsGeneralError = false;
            return true;
        }

        private async void ShowErrorMessage(string message)
        {
            MessageDialog msg = new MessageDialog(message);
            await msg.ShowAsync();
        }

        private async void HandleDataMessage(Deck deckInstance, Flashcard flashcardInstance)
        {
            if (deckInstance != null)
            {
                if (flashcardInstance != null)
                {
                    _mode = Mode.EditFlashcard;
                    Question = flashcardInstance.Question;
                    Answer = flashcardInstance.Answer;

                    HeaderMessage = StringResources.GetString("EditFlashcard");
                    SubmitFormMessage = StringResources.GetString("Edit"); ;

                    var tips = await _restService.GetTips(deckInstance.ID, flashcardInstance.Id);
                    if (tips != null)
                    {
                        foreach (var tip in tips)
                        {
                            TipsCollection.Add(new TipViewModel(tip.ID, tip.Prompt));
                        }
                    }


                    _flashcard = flashcardInstance;
                }
                else
                {
                    _mode = Mode.AddNewFlashcardToDeck;
                    Question = "";
                    Answer = "";

                    _flashcard = new Flashcard();
                    HeaderMessage = StringResources.GetString("AddFlashcard");
                    SubmitFormMessage = StringResources.GetString("AddFlashcard");
                }

                _deck = deckInstance;

                
                ShowDeckName = false;

                TipsCollection = new ObservableCollection<TipViewModel>();
            }
            else
            {
                _mode = Mode.CreateFlashcardAndDeck;
                Question = "";
                Answer = "";
                DeckName = "";
                _deck = new Deck();
                _flashcard = new Flashcard();
                HeaderMessage = StringResources.GetString("CreateNewDeck");
                SubmitFormMessage = StringResources.GetString("CreateNewDeck");
                ShowDeckName = true;

                TipsCollection = new ObservableCollection<TipViewModel>();
            }
            IsGeneralError = false;
        }
    }
}
