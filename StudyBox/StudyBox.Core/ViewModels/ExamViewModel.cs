using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using GalaSoft.MvvmLight.Command;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using StudyBox.Core.Services;

namespace StudyBox.Core.ViewModels
{
    public class ExamViewModel : ExtendedViewModelBase
    {
        private IRestService _restService;
        private IStatisticsDataService _statisticsService;
        private Deck _deckInstance;
        private List<Flashcard> _flashcards;
        private List<Flashcard> _badAnswerFlashcards;
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

        public ExamViewModel(INavigationService navigationService, IRestService restService, IStatisticsDataService statisticsService) : base(navigationService)
        {
            _restService = restService;
            _statisticsService = statisticsService;
            Messenger.Default.Register<DataMessageToExam>(this, x => HandleDataMessage(x.DeckInstance, x.BadAnswerFlashcards));
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
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : _flashcards[_numberOfCurrentFlashcard].Question;
            }
        }

        public string Answer
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : _flashcards[_numberOfCurrentFlashcard].Answer;
            }
        }

        public string Hint
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? string.Empty : (!_isHintAlreadyShown ? StringResources.GetString("HintRectangle") : _flashcards[_numberOfCurrentFlashcard].Hint);
            }
        }

        public bool IsHintAvailable
        {
            get
            {
                return _flashcards == null || _flashcards.Count == 0 ? false : !string.IsNullOrEmpty(_flashcards[_numberOfCurrentFlashcard].Hint);
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
                    _flashcards = await _restService.GetFlashcards(_deckInstance.ID);
                else
                    _flashcards = badAnswerFlashcards;
                _badAnswerFlashcards = new List<Flashcard>();
                
                //MOCK-UP:
                //_flashcards = new List<Flashcard>()
                //{
                //    new Flashcard("1", new Deck(), "Question?", "Answer?", "Hint"),
                //    new Flashcard("2", new Deck(), "Question2?", "Answer2?", "Hint2"),
                //    new Flashcard("3", new Deck(), "Question3?", "Answer3?", "Hint3")
                //};

                if (_flashcards != null && _flashcards.Count > 0)
                {
                    if(!IsQuestionVisible)
                    {
                        ShowQuestionView();
                    }
                    else
                    {
                        _isHintAlreadyShown = false;
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
            RaisePropertyChanged("Hint");
            RaisePropertyChanged("IsHintAvailable");
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
            
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    _statisticsService.IncrementGoodAnswers();
                    for(int k = 0; k < 200; k++)
                        _statisticsService.IncrementAnswers();
                    _statisticsService.IncrementCountOfDecks(_deckInstance);
                    for(int z=0;z<1;z++)
                        _statisticsService.IncrementTestsCountAnswers();
                    
                }   
            }
            
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

        private void ShowAvailableHint()
        {
            _isHintAlreadyShown = true;
            RaisePropertyChanged("Hint");
        }

        private void ShowQuestionView()
        {
            Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowQuestion" });
            _isHintAlreadyShown = false;
            IsQuestionVisible = true;
            RaiseAllPropertiesChanged();
        }
    }
}
