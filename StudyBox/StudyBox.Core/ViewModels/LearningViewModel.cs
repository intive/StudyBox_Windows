using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.ViewModels
{
    public class LearningViewModel : ExtendedViewModelBase
    {
        private IRestService _restService;
        private Deck _deckInstance;
        private List<Flashcard> _flashcards;
        private string _nameOfDeck;
        private bool _isDataLoading;
        private bool _isFinishLearning;
        private bool _isQuestionVisible = true;
        private int _numberOfCurrentFlashcard = 0;
        private RelayCommand _showAnswer;
        private RelayCommand _goodAnswer;
        private RelayCommand _badAnswer;
        private RelayCommand _skipFlashcard;
        private RelayCommand _repeatLearning;
        private RelayCommand _dontRepeatLearning;

        public LearningViewModel(INavigationService navigationService, IRestService restService) : base(navigationService)
        {
            _restService = restService;
            Messenger.Default.Register<DataMessageToExam>(this, x => HandleDataMessage(x.DeckInstance));
        }

        public RelayCommand BadAnswer
        {
            get
            {
                return _badAnswer ?? (_badAnswer = new RelayCommand(HandleBadAnswer));
            }
        }

        public RelayCommand GoodAnswer
        {
            get
            {
                return _goodAnswer ?? (_goodAnswer = new RelayCommand(HandleGoodAnswer));
            }
        }

        public RelayCommand SkipFlashcard
        {
            get
            {
                return _skipFlashcard ?? (_skipFlashcard = new RelayCommand(HandleSkipFlashcard));
            }
        }

        public RelayCommand RepeatLearning
        {
            get
            {
                return _repeatLearning ?? (_repeatLearning = new RelayCommand(StartLearning));
            }
        }

        public RelayCommand DontRepeatLearning
        {
            get
            {
                return _dontRepeatLearning ?? (_dontRepeatLearning = new RelayCommand(LeaveLearning));
            }
        }

        public RelayCommand ShowAnswer
        {
            get
            {
                return _showAnswer ?? (_showAnswer = new RelayCommand(SwipeAndShowAnswer));
            }
        }

        public string NameOfDeck
        {
            get
            {
                return _nameOfDeck;
            }
            set
            {
                if (_nameOfDeck != value)
                {
                    _nameOfDeck = value;
                    RaisePropertyChanged("NameOfDeck");
                }
            }
        }

        public string NumberOfFlashcard
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : string.Format("{0} {1} {2}", (_numberOfCurrentFlashcard + 1), StringResources.GetString("OutOf"), _flashcards.Count.ToString());
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
                    RaisePropertyChanged("IsDataLoading");
                }
            }
        }

        public bool IsQuestionVisible
        {
            get
            {
                return _isQuestionVisible;
            }
            set
            {
                if (_isQuestionVisible != value)
                {
                    _isQuestionVisible = value;
                    RaisePropertyChanged("IsQuestionVisible");
                }
            }
        }

        public string Question
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : _flashcards.First().Question;
            }
        }

        public string Answer
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : _flashcards.First().Answer;
            }
        }

        public bool AreAnyFlashcards
        {
            get
            {
                return !_isDataLoading && (_flashcards != null && _flashcards.Count > 0);
            }
        }

        public bool ShowInformationAboutNoFlashcards
        {
            get
            {
                return !_isDataLoading && (_flashcards == null || _flashcards.Count == 0);
            }
        }

        public bool IsFinishLearning
        {
            get
            {
                return _isFinishLearning;
            }
            set
            {
                if (_isFinishLearning != value)
                {
                    _isFinishLearning = value;
                    RaisePropertyChanged("AreAnyFlashcards");
                    RaisePropertyChanged("IsFinishLearning");
                }
            }
        }

        private async void HandleDataMessage(Deck deckInstance)
        {
            IsFinishLearning = false;
            IsQuestionVisible = true;

            if (deckInstance != null)
            {
                _numberOfCurrentFlashcard = 0;
                _deckInstance = deckInstance;
                NameOfDeck = _deckInstance.Name;

                IsDataLoading = true;

                try
                {
                    _flashcards = await _restService.GetFlashcards(_deckInstance.ID);
                }
                catch (Exception)
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("OperationFailed")));
                }

                //MOCK-UP:
                //_flashcards = new List<Flashcard>()
                //{
                //    new Flashcard("1", new Deck(), "Q1?", "A1", "H1"),
                //    new Flashcard("2", new Deck(), "Q2?", "A2", "H2"),
                //    new Flashcard("3", new Deck(), "Q3?", "A3", "H3"),
                //    new Flashcard("4", new Deck(), "Q4?", "A4", "H4"),
                //    new Flashcard("5", new Deck(), "Q5?", "A5", "H5")
                //};

                if (_flashcards != null && _flashcards.Count > 0)
                {
                    if (!IsQuestionVisible)
                    {
                        ShowQuestionView();
                    }
                    else
                    {
                        RaiseAllPropertiesChanged();
                    }
                }

                IsDataLoading = false;
                RaisePropertyChanged("AreAnyFlashcards");
                RaisePropertyChanged("ShowInformationAboutNoFlashcards");
            }
        }

        private void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged("Answer");
            RaisePropertyChanged("Question");
            RaisePropertyChanged("NumberOfFlashcard");
            RaisePropertyChanged("AreAnyFlashcards");
            RaisePropertyChanged("ShowInformationAboutNoFlashcards");
        }

        private void SwipeAndShowAnswer()
        {
            Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowAnswer" });
            IsQuestionVisible = false;
        }

        private void HandleSkipFlashcard()
        {
            Flashcard _tempFlashcard = _flashcards.First();
            _flashcards.Remove(_flashcards.First());
            _flashcards.Add(_tempFlashcard);

            ShowNexFlashCardOrGoToResults();
        }

        private void HandleGoodAnswer()
        {
            _flashcards.Remove(_flashcards.First());
            RaisePropertyChanged("AreAnyFlashcards");
            ShowNexFlashCardOrGoToResults();
        }

        private void HandleBadAnswer()
        {
            Flashcard currentFlashcard = _flashcards.First();

            int numberOfSwipeFlashcard = _numberOfCurrentFlashcard + (_flashcards.Count - _numberOfCurrentFlashcard) / 2;
            Flashcard swipeFlashcard = _flashcards[numberOfSwipeFlashcard];
            _flashcards[0] = swipeFlashcard;
            _flashcards[numberOfSwipeFlashcard] = currentFlashcard;

            ShowNexFlashCardOrGoToResults();
        }

        private void ShowNexFlashCardOrGoToResults()
        {
            _numberOfCurrentFlashcard++;
            if (_numberOfCurrentFlashcard >= _flashcards.Count)
                _numberOfCurrentFlashcard = 0;

            if (_flashcards.Count != 0)
            {
                ShowQuestionView();
            }
            else
            {
                _flashcards.Clear();
                IsFinishLearning = true;
            }
        }
        private void ShowQuestionView()
        {
            Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowQuestion" });
            IsQuestionVisible = true;
            RaiseAllPropertiesChanged();
        }

        private void StartLearning()
        {
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance));
        }

        private void LeaveLearning()
        {
            NavigationService.NavigateTo("DecksListView");
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false));
        }
    }
}
