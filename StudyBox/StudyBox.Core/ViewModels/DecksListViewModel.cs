using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class DecksListViewModel : ViewModelBase
    {
        #region fields
        private ObservableCollection<Deck> _decksCollection;
        private INavigationService _navigationService;
        private IRestService _restService;
        private bool _isDataLoading=false;
        #endregion

        #region Constructors
        public DecksListViewModel(INavigationService navigationService, IRestService restService)
        {
            this._navigationService = navigationService;
            this._restService = restService;
            DecksCollection = new ObservableCollection<Deck>();
            InitializeDecksCollection();

            TapTileCommand = new RelayCommand<string>(TapTile);
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false, false));
        }
        #endregion

        #region Getters/Setters
        public ObservableCollection<Deck> DecksCollection
        {
            get
            {
                return _decksCollection;
            }
            set
            {
                if (_decksCollection != value)
                {
                    _decksCollection = value;
                    RaisePropertyChanged();
                }
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
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Methods
        private async void InitializeDecksCollection()
        {         
            List<Deck> _deckLists = new List<Deck>();
            IsDataLoading = true;
            _deckLists = await _restService.GetDecks();
            _deckLists.ForEach(x=> DecksCollection.Add(x));
            IsDataLoading = false;
        }
        #endregion


        #region Buttons

        public ICommand TapTileCommand { get; set; }
        private void TapTile(string id)
        {
            _navigationService.NavigateTo("ExamView");
            Deck deck = DecksCollection.Where(x => x.ID == id).FirstOrDefault();
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, true, false, false));
        }

        #endregion


    }
}
