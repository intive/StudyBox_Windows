using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using GalaSoft.MvvmLight.Command;
using Windows.ApplicationModel.Resources;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class ExamViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IRestService _restService;
        private Deck _deckInstance;
        private List<Flashcard> _flashcards;
        private string _nameOfDeck;
        private bool _isDataLoading;
        private bool _isHintAlreadyShown = false;
        private bool _isQuestionVisible = true;
        private int _numberOfCurrentFlashcard = 0;
        private int _numberOfCorrectAnswers = 0;
        private RelayCommand _showAnswer;
        private RelayCommand _showHint;
        private RelayCommand _countGoodAnswer;
        private RelayCommand _countBadAnswer;
        private ResourceLoader _stringResources;

        public ExamViewModel(INavigationService navigationService, IRestService restService)
        {
            _navigationService = navigationService;
            _restService = restService;

            Messenger.Default.Register<DataMessageToExam>(this, x => HandleDataMessage(x.DeckInstance));
            _stringResources = new ResourceLoader();
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

        public RelayCommand ShowHint
        {
            get
            {
                return _showHint ?? (_showHint = new RelayCommand(ShowAvailableHint, () => IsHintAvailable));
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
                return string.Format("#{0}", (_numberOfCurrentFlashcard + 1));
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
                return _flashcards == null ? string.Empty : _flashcards[_numberOfCurrentFlashcard].Question;
            }
        }

        public string Answer
        {
            get
            {
                return _flashcards == null ? string.Empty : _flashcards[_numberOfCurrentFlashcard].Answer;
            }
        }

        public string Hint
        {
            get
            {
                return _flashcards == null ? string.Empty : (!_isHintAlreadyShown ? _stringResources.GetString("HintRectangle") : _flashcards[_numberOfCurrentFlashcard].Hint);
            }
        }

        public bool IsHintAvailable
        {
            get
            {
                return _flashcards == null ? false : !string.IsNullOrEmpty(_flashcards[_numberOfCurrentFlashcard].Hint);
            }
        }

        public string CurrentResult
        {
            get
            {
                return string.Format("{0}/{1}", _numberOfCorrectAnswers, _flashcards == null ? "0" : _flashcards.Count.ToString());
            }
        }

        private async void HandleDataMessage(Deck deckInstance)
        {
            if (deckInstance != null)
            {
                _numberOfCurrentFlashcard = 0;
                _numberOfCorrectAnswers = 0;

                _deckInstance = deckInstance;
                NameOfDeck = _deckInstance.Name;

                IsDataLoading = true;
                //_flashcards = await _restService.GetFlashcards(_deckInstance.ID);

                //MOCK-UP:
                _flashcards = new List<Flashcard>()
                {
                    new Flashcard("1", new Deck(), "Question?", "Answer?", "Hint"),
                    new Flashcard("2", new Deck(), "Question2?", "Answer2?", "Hint2"),
                    new Flashcard("3", new Deck(), "Question3?", "Answer3?", "Hint3")
                };

                RaiseAllPropertiesChanged();
                IsDataLoading = false;
            }
        }

        private void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged("Answer");
            RaisePropertyChanged("Question");
            RaisePropertyChanged("Hint");
            RaisePropertyChanged("IsHintAvailable");
            RaisePropertyChanged("CurrentResult");
            RaisePropertyChanged("NumberOfFlashcard");
        }

        private void SwipeAndShowAnswer()
        {
            Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowAnswer" });
            IsQuestionVisible = false;
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
                Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowQuestion" });
                _isHintAlreadyShown = false;
                IsQuestionVisible = true;
                RaiseAllPropertiesChanged();
            }
            else
            {
                _navigationService.NavigateTo("SummaryView");
                Messenger.Default.Send<DataMessageToSummary>(new DataMessageToSummary(new Exam { CorrectAnswers = _numberOfCorrectAnswers, Questions = _flashcards.Count }));
                Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance));
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, true, false, false));
            }
        }

        private void CountBadAnswerAndShowNextFlashcard()
        {
            ShowNexFlashCardOrGoToResults();
        }

        private void ShowAvailableHint()
        {
            _isHintAlreadyShown = true;
            RaisePropertyChanged("Hint");
        }
    }
}
