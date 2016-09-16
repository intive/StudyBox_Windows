using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using GalaSoft.MvvmLight.Command;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using StudyBox.Core.Services;
using System;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace StudyBox.Core.ViewModels
{
    public class ExamViewModel : ExtendedViewModelBase
    {
        #region Private Fields
        private IRestService _restService;
        private IInternetConnectionService _internetConnectionService;
        private IStatisticsDataService _statisticsService;
        private Deck _deckInstance;
        private List<Flashcard> _flashcards;
        private List<Flashcard> _badAnswerFlashcards;
        private List<Tip> Hints;
        private string _nameOfDeck;
        private string _mainRectangleWithQuestionOrHint;
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

        private RelayCommand<object> _gotFocusCommand;
        private RelayCommand<KeyRoutedEventArgs> _detectKeyDownCommand;
        #endregion

        #region Constructor
        public ExamViewModel(INavigationService navigationService, IRestService restService, IStatisticsDataService statisticsService, IInternetConnectionService internetConnectionService, IDetectKeysService detectKeysService) : base(navigationService, detectKeysService)
        {
            _restService = restService;
            _internetConnectionService = internetConnectionService;
            _statisticsService = statisticsService;
            Messenger.Default.Register<DataMessageToExam>(this, x => HandleDataMessage(x.DeckInstance, x.BadAnswerFlashcards));
        }
        #endregion
        public RelayCommand<KeyRoutedEventArgs> DetectKeyDownCommand
        {
            get { return _detectKeyDownCommand ?? (_detectKeyDownCommand = new RelayCommand<KeyRoutedEventArgs>(DetectKeysService.DetectKeyDown)); }
        }
        public RelayCommand<object> GotFocusCommand
        {
            get { return _gotFocusCommand ?? (_gotFocusCommand = new RelayCommand<object>(DetectKeysService.GotFocus)); }
        }
        #region Commands
        public RelayCommand LeftArrowClicked
        {
            get
            {
                return _leftArrowClicked ?? (_leftArrowClicked = new RelayCommand(GetPreviousHint));
            }
        }

        public RelayCommand RightArrowClicked
        {
            get
            {
                return _rightArrowClicked ?? (_rightArrowClicked = new RelayCommand(GetNextHint));
            }
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
        #endregion

        #region Other Properties
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
                return _flashcards != null && _flashcards.Count > 0 && CurrentlyVisibleHint && Hints != null && Hints.Count > 0 && _numberOfCurrentHint > 0;
            }
        }

        public bool IsRightArrowVisible
        {
            get
            {
                return _flashcards != null && _flashcards.Count > 0 && CurrentlyVisibleHint && Hints != null && Hints.Count > 0 && _numberOfCurrentHint < Hints.Count - 1;
            }
        }

        public bool ShowInformationAboutNoFlashcards
        {
            get
            {
                return !_isDataLoading && (_flashcards == null || _flashcards.Count == 0);
            }
        }
        #endregion

        #region Private Methods
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
                    try
                    {
                        _flashcards = await _restService.GetFlashcards(_deckInstance.ID);
                        if (_flashcards != null && _flashcards.Count > 0)
                        {
                            string deckId = _deckInstance.ID;
                            for (int i = 0; i < _flashcards.Count; i++)
                            {
                                var tips = await _restService.GetTips(deckId, _flashcards[i].Id);
                                if (tips != null && tips.Count > 0)
                                {
                                    _flashcards[i].Tips = tips;
                                    _flashcards[i].TipsCount = tips.Count;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, ex.Message));
                    }
                }
                else
                {
                    _flashcards = badAnswerFlashcards;
                }

                _badAnswerFlashcards = new List<Flashcard>();
                
                if (_flashcards != null && _flashcards.Count > 0)
                {
                    //if (!IsQuestionVisible)
                    //{
                    //    ShowQuestionView();
                    //}
                    //else
                    //{
                        ShowQuestionInMainRectangle();
                        RaiseAllPropertiesChanged();
                    //}
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
            RaisePropertyChanged("IsRightArrowVisible");
            RaisePropertyChanged("IsLeftArrowVisible");
            RaisePropertyChanged("BottomRectangleText");
        }

        private async void SwipeAndShowAnswer()
        {
            if (!CurrentlyVisibleHint)
            {
                Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowAnswer" });
                await Task.Delay(TimeSpan.FromMilliseconds(450));
                IsQuestionVisible = false;
            }
        }

        private void CountGoodAnswerAndShowNextFlashcard()
        {
            _numberOfCorrectAnswers++;
            _statisticsService.IncrementAnswers();
            _statisticsService.IncrementGoodAnswers();
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
                MainRectangleWithQuestionOrHint = string.Empty;
                _statisticsService.SaveTestsHistory(new TestsHistory(DateTime.Now.ToString("g"),_nameOfDeck,_numberOfCorrectAnswers,_numberOfCurrentFlashcard));
                NavigationService.NavigateTo("SummaryView");
                Messenger.Default.Send<DataMessageToSummary>(new DataMessageToSummary(new Exam { CorrectAnswers = _numberOfCorrectAnswers, Questions = _flashcards.Count }));
                Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance, _badAnswerFlashcards));
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false, NameOfDeck));
            }
        }

        private void CountBadAnswerAndShowNextFlashcard()
        {
            _badAnswerFlashcards.Add(_flashcards[_numberOfCurrentFlashcard]);
            _statisticsService.IncrementAnswers();
            ShowNexFlashCardOrGoToResults();
        }

        private async void ShowAvailableHintOrQuestion()
        {
            if (!CurrentlyVisibleHint)
            {
                CurrentlyVisibleHint = true;
                _numberOfCurrentHint = 0;

                if (!await _internetConnectionService.IsNetworkAvailable())
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, true, true,
                        StringResources.GetString("NoInternetConnection")));
                    return;
                }
                else if (!_internetConnectionService.IsInternetAccess())
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("AccessDenied")));
                    return;
                }
                else
                {
                    try
                    {
                        Hints = await _restService.GetTips(_deckInstance.ID, _flashcards[_numberOfCurrentFlashcard].Id);
                        ShowNewHint();
                    }
                    catch (Exception ex)
                    {
                        Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, ex.Message));
                    }
                }
            }
            else
            {
                ShowQuestionInMainRectangle();
            }

            RaisePropertyChanged("IsRightArrowVisible");
            RaisePropertyChanged("IsLeftArrowVisible");
            RaisePropertyChanged("BottomRectangleText");
        }

        private async void ShowQuestionView()
        {
            Messenger.Default.Send(new StartStoryboardMessage { StoryboardName = "TurnFlashcardToShowQuestion" });
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            ShowQuestionInMainRectangle();
            RaiseAllPropertiesChanged();
        }

        private void ShowQuestionInMainRectangle()
        {
            CurrentlyVisibleHint = false;
            IsQuestionVisible = true;
            MainRectangleWithQuestionOrHint = _flashcards[_numberOfCurrentFlashcard].Question;
        }

        private void GetNextHint()
        {
            _numberOfCurrentHint++;
            ShowNewHint();
        }

        private void GetPreviousHint()
        {
            _numberOfCurrentHint--;
            ShowNewHint();
        }

        private void ShowNewHint()
        {
            MainRectangleWithQuestionOrHint = Hints[_numberOfCurrentHint].Essence;
            RaisePropertyChanged("IsLeftArrowVisible");
            RaisePropertyChanged("IsRightArrowVisible");
        }
        #endregion
    }
}
