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

        public LearningViewModel(INavigationService navigationService, IRestService restService) : base(navigationService)
        {
            _restService = restService;
        }
    }
}
