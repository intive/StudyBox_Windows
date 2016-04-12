using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using StudyBox.Core.Models;

namespace StudyBox.Core.ViewModels
{
    public class DecksListViewModel : ExtendedViewModelBase
    {
        #region fields
        private ObservableCollection<Deck> _decksCollection;
        private IRestService _restService;
        private bool _isDataLoading=false;
        private bool _isSearchMessageVisible = false;
        #endregion

        #region Constructors
        public DecksListViewModel(INavigationService navigationService, IRestService restService) : base(navigationService)
        {
            Messenger.Default.Register<ReloadMessageToDecksList>(this,x=> HandleReloadMessage(x.Reload));
            Messenger.Default.Register<SearchMessageToDeckList>(this, x => HandleSearchMessage(x.SearchingContent));
            this._restService = restService;
            DecksCollection = new ObservableCollection<Deck>();

            InitializeDecksCollection();

            TapTileCommand = new RelayCommand<string>(TapTile);
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false));
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

        public bool SearchMessageVisibility
        {
            get
            {
                return _isSearchMessageVisible;
            }
            set
            {
                if (_isSearchMessageVisible != value)
                {
                    _isSearchMessageVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Methods
        private async void InitializeDecksCollection()
        {
            if (await checkInternetConnection())
            {
                List<Deck> _deckLists = new List<Deck>();
                IsDataLoading = true;
                _deckLists = await _restService.GetDecks();
                if(_deckLists!=null)
                {
                    _deckLists.Sort((x, y) => string.Compare(x.Name, y.Name));
                    _deckLists.ForEach(x => DecksCollection.Add(x));
                }
                IsDataLoading = false;
            }
        }

        private void HandleReloadMessage(bool reload)
        {
            if (reload)
            {
                DecksCollection.Clear();
                SearchMessageVisibility = false;
                InitializeDecksCollection();
            }
        }

        private async void HandleSearchMessage(string searchingContent)
        {
            if (await checkInternetConnection())
            {
                DecksCollection.Clear();
                SearchMessageVisibility = false;
                IsDataLoading = true;
                List<Deck> searchList = await _restService.GetAllDecks(false, true, searchingContent);
                if (searchList != null)
                {
                    searchList.ForEach(x => DecksCollection.Add(x));
                    IsDataLoading = false;
                }
                else
                {
                    IsDataLoading = false;
                    SearchMessageVisibility = true;
                }
            }
        }

        private async Task<bool> checkInternetConnection()
        {
            if (!(await Task.Run(() => NetworkInterface.GetIsNetworkAvailable())))
            {
                MessageDialog msg = new MessageDialog(StringResources.GetString("NoInternetConnection"));
                await msg.ShowAsync();
                return false;
            }
            else if (!(NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess))
            {
                MessageDialog msg = new MessageDialog(StringResources.GetString("AccessDenied"));
                await msg.ShowAsync();
                return false;
            }
            else
                return true;
        }
        #endregion


        #region Buttons

        public ICommand TapTileCommand { get; set; }
        private void TapTile(string id)
        {
            NavigationService.NavigateTo("ExamView");
            Deck deck = DecksCollection.Where(x => x.ID == id).FirstOrDefault();
            Messenger.Default.Send<DataMessageToExam>(new DataMessageToExam(deck));
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true,false,false,deck.Name));
        }

        #endregion
    }
}
