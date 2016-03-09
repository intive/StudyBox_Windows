﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using StudyBox.View;
using Windows.ApplicationModel.Resources;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Messages;
using StudyBox.Model;

namespace StudyBox.ViewModel
{
    public class SummaryViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private ResourceLoader _stringResources;
        private Exam _exam;
        private Deck _deckInstance;
        private double _lowResult = 0.35;
        private double _highResult = 0.75;
        private RelayCommand _goToDecks;
        private RelayCommand _improveResults;

        public SummaryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _stringResources = new ResourceLoader();
            Messenger.Default.Register<DataMessageToSummary>(this, x => { _exam = x.ExamInstance; });
            Messenger.Default.Register<DataMessageToExam>(this, x => { _deckInstance = x.DeckInstance; });
        }

        public RelayCommand GoToDecks
        {
            get
            {
                return _goToDecks ?? (_goToDecks = new RelayCommand(GoBackToDecks));
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
            get
            {
                return ConcludeResult(_exam);
            }
        }

        public string Score
        {
            get
            {
                return string.Format("{0}/{1}", _exam.CorrectAnswers, _exam.Questions);
            }
        }

        private void GoBackToDecks()
        {
            _navigationService.NavigateTo("DecksListView");
        }

        private void TryImproveResults()
        {
            _navigationService.NavigateTo("ExamView");
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance));
        }

        private string ConcludeResult(Exam exam)
        {
            double percent = Convert.ToDouble(exam.CorrectAnswers) / Convert.ToDouble(exam.Questions);
            if (percent < _lowResult)
                return _stringResources.GetString("TryAgain");
            else if (percent > _highResult)
                return _stringResources.GetString("Congrats");
            else
                return _stringResources.GetString("NotBad");
        }

    }
}