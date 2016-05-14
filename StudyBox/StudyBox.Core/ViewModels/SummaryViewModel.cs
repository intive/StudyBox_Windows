using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;
using System.Collections.Generic;
using StudyBox.Core.Interfaces;

namespace StudyBox.Core.ViewModels
{
    public class SummaryViewModel : ExtendedViewModelBase
    {
        private Deck _deckInstance;
        private List<Flashcard> _badAnswerFlashcards;
        private const double _lowResult = 0.34;
        private const double _highResult = 0.67;
        private string _congrats;
        private string _score;
        private string _repeatTest;
        private bool _worstTestVisibility;
        private List<int> _resultList;
        private RelayCommand _worstTest;
        private RelayCommand _improveResults;
        private IStatisticsDataService _statisticsService;

        public SummaryViewModel(INavigationService navigationService, IStatisticsDataService statisticsService) : base(navigationService)
        {
            _statisticsService = statisticsService;
            Messenger.Default.Register<DataMessageToSummary>(this, x => { CalculateResult(x.ExamInstance); });
            Messenger.Default.Register<DataMessageToExam>(this, x => { _deckInstance = x.DeckInstance; _badAnswerFlashcards = x.BadAnswerFlashcards; });
        }

        public RelayCommand WorstTest
        {
            get
            {
                return _worstTest ?? (_worstTest = new RelayCommand(GoToWorstTest));
            }
        }

        public RelayCommand ImproveResults
        {
            get
            {
                return _improveResults ?? (_improveResults = new RelayCommand(TryImproveResults));
            }
        }

        public string Congrats
        {
            get { return _congrats; }
            set
            {
                if (_congrats != value)
                {
                    _congrats = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Score
        {
            get { return _score; }
            set
            {
                if (_score != value)
                {
                    _score = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string RepeatTest
        {
            get { return _repeatTest; }
            set
            {
                if (_repeatTest != value)
                {
                    _repeatTest = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<int> ResultList
        {
            get { return _resultList; }
            set
            {
                if (_resultList != value)
                {
                    _resultList = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool WorstTestVisibility
        {
            get { return _worstTestVisibility; }
            set
            {
                if (_worstTestVisibility != value)
                {
                    _worstTestVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void CalculateResult(Exam exam)
        {
            double _result = Convert.ToDouble(exam.CorrectAnswers) / Convert.ToDouble(exam.Questions);
            Score = string.Format("{0}/{1}", exam.CorrectAnswers, exam.Questions);
            if (_result < _lowResult)
                Congrats = StringResources.GetString("TryAgain");
            else if (_result >= _highResult)
                Congrats = StringResources.GetString("Congrats");
            else
                Congrats = StringResources.GetString("NotBad");
            if (_result == 1)
            {
                RepeatTest = StringResources.GetString("RepeatTest");
                WorstTestVisibility = false;
            }
            else
            {
                RepeatTest = StringResources.GetString("ImproveResults");
                WorstTestVisibility = true;
            }
            _resultList = new List<int>();
            _resultList.Add(exam.CorrectAnswers);
            _resultList.Add(exam.Questions - exam.CorrectAnswers);
            ResultList = _resultList;

        }

        private void GoToWorstTest()
        {
            NavigationService.NavigateTo("ExamView");
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance, _badAnswerFlashcards));
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, _deckInstance.Name));
            _statisticsService.IncrementTestsCountAnswers();
        }

        private void TryImproveResults()
        {
            NavigationService.NavigateTo("ExamView");
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance));
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, _deckInstance.Name));
            _statisticsService.IncrementTestsCountAnswers();
        }
    }
}