using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class SummaryViewModel : ExtendedViewModelBase
    {
        private Exam _exam;
        private Deck _deckInstance;
        private double _lowResult = 0.35;
        private double _highResult = 0.75;
        private RelayCommand _goToDecks;
        private RelayCommand _improveResults;

        public SummaryViewModel(INavigationService navigationService) : base(navigationService)
        {
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
            NavigationService.NavigateTo("DecksListView");
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false, false));
        }

        private void TryImproveResults()
        {
            NavigationService.NavigateTo("ExamView");
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(_deckInstance));
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, true, false, false));
        }

        private string ConcludeResult(Exam exam)
        {
            double percent = Convert.ToDouble(exam.CorrectAnswers) / Convert.ToDouble(exam.Questions);
            if (percent < _lowResult)
                return StringResources.GetString("TryAgain");
            else if (percent > _highResult)
                return StringResources.GetString("Congrats");
            else
                return StringResources.GetString("NotBad");
        }
    }
}