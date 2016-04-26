using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using GalaSoft.MvvmLight.Command;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using System;

namespace StudyBox.Core.ViewModels
{
    public class ExamViewModel : ExtendedViewModelBase
    {
        private IRestService _restService;
        private Deck _deckInstance;
        private List<Flashcard> _flashcards;
        private List<Flashcard> _badAnswerFlashcards;
        private List<string> Hints;
        private string _nameOfDeck;
        private bool _isDataLoading;
        private bool _currentlyVisibleHint = false;
        private bool _isQuestionVisible = true;
        private int _numberOfCurrentHint = 0;
        private int _numberOfCurrentFlashcard = 0;
        private int _numberOfCorrectAnswers = 0;
        private RelayCommand _showAnswer;
        private RelayCommand _showHintOrQuestion;
        private RelayCommand _countGoodAnswer;
        private RelayCommand _countBadAnswer;
        private RelayCommand _leftArrowClicked;
        private RelayCommand _rightArrowClicked;

        public ExamViewModel(INavigationService navigationService, IRestService restService) : base(navigationService)
        {
            _restService = restService;

            Messenger.Default.Register<DataMessageToExam>(this, x => HandleDataMessage(x.DeckInstance, x.BadAnswerFlashcards));
        }

        public RelayCommand LeftArrowClicked
        {
            get
            {
                return _leftArrowClicked ?? (_leftArrowClicked = new RelayCommand(GetPreviousHint));
            }
        }

        private void GetNextHint()
        {
            _numberOfCurrentHint++;
            MainRectangleWithQuestionOrHint = Hints[_numberOfCurrentHint];
            RaisePropertyChanged("IsLeftArrowVisible");
            RaisePropertyChanged("IsRightArrowVisible");
        }

        public RelayCommand RightArrowClicked
        {
            get
            {
                return _rightArrowClicked ?? (_rightArrowClicked = new RelayCommand(GetNextHint));
            }
        }

        private void GetPreviousHint()
        {
            _numberOfCurrentHint--;
            MainRectangleWithQuestionOrHint = Hints[_numberOfCurrentHint];
            RaisePropertyChanged("IsLeftArrowVisible");
            RaisePropertyChanged("IsRightArrowVisible");
        }

        public RelayCommand CountBadAnswer
        {
            get
            {
                return _countBadAnswer ?? (_countBadAnswer = new RelayCommand(CountBadAnswerAndShowNextFlashcard));
            }
        }

        public RelayCommand CountGoodAnswer
        {
            get
            {
                return _countGoodAnswer ?? (_countGoodAnswer = new RelayCommand(CountGoodAnswerAndShowNextFlashcard));
            }
        }

        public RelayCommand ShowAnswer
        {
            get
            {
                return _showAnswer ?? (_showAnswer = new RelayCommand(SwipeAndShowAnswer));
            }
        }

        public RelayCommand ShowHintOrQuestion
        {
            get
            {
                return _showHintOrQuestion ?? (_showHintOrQuestion = new RelayCommand(ShowAvailableHintOrQuestion, () => IsHintAvailable));
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

        public string BottomRectangleText
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : (CurrentlyVisibleHint ? StringResources.GetString("Question") : StringResources.GetString("Hint"));
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

        private string _mainRectangleWithQuestionOrHint;
        public string MainRectangleWithQuestionOrHint
        {
            get
            {
                return _mainRectangleWithQuestionOrHint;
            }
            set
            {
                _mainRectangleWithQuestionOrHint = value;
                RaisePropertyChanged("MainRectangleWithQuestionOrHint");
            }
        }

        public string Answer
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : _flashcards[_numberOfCurrentFlashcard].Answer;
            }
        }

        public bool IsHintAvailable
        {
            get
            {
                return _flashcards != null && _flashcards.Count > 0 && _flashcards[_numberOfCurrentFlashcard].TipsCount > 0;
            }
        }

        public bool CurrentlyVisibleHint
        {
            get
            {
                return _currentlyVisibleHint;
            }
            set
            {
                _currentlyVisibleHint = value;
                RaisePropertyChanged("CurrentlyVisibleHint");
            }
        }

        public bool AreAnyFlashcards
        {
            get
            {
                return !_isDataLoading && (_flashcards != null && _flashcards.Count > 0);
            }
        }

        public bool IsLeftArrowVisible
        {
            get
            {
                return _flashcards != null && _flashcards.Count > 0 && _currentlyVisibleHint && Hints != null && Hints.Count > 0 && _numberOfCurrentHint > 0;
            }
        }

        public bool IsRightArrowVisible
        {
            get
            {
                return _flashcards != null && _flashcards.Count > 0 && _currentlyVisibleHint && Hints != null && Hints.Count > 0 && _numberOfCurrentHint < Hints.Count - 1;
            }
        }

        public bool ShowInformationAboutNoFlashcards
        {
            get
            {
                return !_isDataLoading && (_flashcards == null || _flashcards.Count == 0);
            }
        }

        private async void HandleDataMessage(Deck deckInstance, List<Flashcard> badAnswerFlashcards)
        {
            if (deckInstance != null)
            {
                _numberOfCurrentFlashcard = 0;
                _numberOfCorrectAnswers = 0;

                _deckInstance = deckInstance;
                NameOfDeck = _deckInstance.Name;

                IsDataLoading = true;

                if (badAnswerFlashcards == null)
                {
                    _flashcards = await _restService.GetFlashcards(_deckInstance.ID);

                    _flashcards = new List<Flashcard>
                    {
                        new Flashcard {Answer="a", Question="q", TipsCount=2 },
                        new Flashcard {Answer="a2", Question="q2", TipsCount=0 }
                    };
                }   
                else
                {
                    _flashcards = badAnswerFlashcards;
                }
                   
                _badAnswerFlashcards = new List<Flashcard>();

                if (_flashcards != null && _flashcards.Count > 0)
                {
                    if (!IsQuestionVisible)
                    {
                        ShowQuestionView();
                    }
                    else
                    {
                        ShowQuestionInMainRectangle();
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
            RaisePropertyChanged("MainRectangleWithQuestionOrHint");
            RaisePropertyChanged("Hint");
            RaisePropertyChanged("IsHintAvailable");
            RaisePropertyChanged("NumberOfFlashcard");
        }

        private void SwipeAndShowAnswer()
        {
            if (!CurrentlyVisibleHint)
            {
                Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowAnswer" });
                IsQuestionVisible = false;
            }
        }

        private void CountGoodAnswerAndShowNextFlashcard()
        {
            _numberOfCorrectAnswers++;
            ShowNexFlashCardOrGoToResults();
        }

        private void ShowNexFlashCardOrGoToResults()
        {
            _numberOfCurrentFlashcard++;
            if (_numberOfCurrentFlashcard < _flashcards.Count)
            {
                ShowQuestionView();
            }
            else
            {
                NavigationService.NavigateTo("SummaryView");
                Messenger.Default.Send<DataMessageToSummary>(new DataMessageToSummary(new Exam { CorrectAnswers = _numberOfCorrectAnswers, Questions = _flashcards.Count }));
                Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance, _badAnswerFlashcards));
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false, NameOfDeck));
            }
        }

        private void CountBadAnswerAndShowNextFlashcard()
        {
            _badAnswerFlashcards.Add(_flashcards[_numberOfCurrentFlashcard]);
            ShowNexFlashCardOrGoToResults();
        }

        private void ShowAvailableHintOrQuestion()
        {
            //_isHintAlreadyShown = true;
            if (!CurrentlyVisibleHint)
            {
                CurrentlyVisibleHint = true;
                _numberOfCurrentHint = 0;
                Hints = new List<string>
                {
                    "hint1",
                    "hint2",
                    "hint3"
                };
                MainRectangleWithQuestionOrHint = Hints[_numberOfCurrentHint];
            }
            else
            {
                ShowQuestionInMainRectangle();
            }

            RaisePropertyChanged("IsRightArrowVisible");
            RaisePropertyChanged("IsLeftArrowVisible");
            RaisePropertyChanged("BottomRectangleText");
        }

        private void ShowQuestionView()
        {
            Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowQuestion" });
            ShowQuestionInMainRectangle();
            IsQuestionVisible = true;
            RaiseAllPropertiesChanged();
        }

        private void ShowQuestionInMainRectangle()
        {
            CurrentlyVisibleHint = false;
            MainRectangleWithQuestionOrHint = _flashcards[_numberOfCurrentFlashcard].Question;
        }
    }
}
