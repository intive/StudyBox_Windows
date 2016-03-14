using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Popups;
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
        private ResourceLoader _stringResources;
        #endregion

        #region Constructors
        public DecksListViewModel(INavigationService navigationService, IRestService restService)
        {
            this._navigationService = navigationService;
            this._restService = restService;
            DecksCollection = new ObservableCollection<Deck>();
            _stringResources=new ResourceLoader();

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
            if (!(await Task.Run(() => NetworkInterface.GetIsNetworkAvailable())))
            {
                MessageDialog msg = new MessageDialog(_stringResources.GetString("NoInternetConnection"));
                await msg.ShowAsync();
            }
            else if (!(NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess))
            {
                MessageDialog msg = new MessageDialog(_stringResources.GetString("AccessDenied"));
                await msg.ShowAsync();
            }
            else
            {
                List<Deck> _deckLists = new List<Deck>();
                IsDataLoading = true;
                _deckLists = await _restService.GetDecks();
                _deckLists.ForEach(x => DecksCollection.Add(x));
                IsDataLoading = false;
            }

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
