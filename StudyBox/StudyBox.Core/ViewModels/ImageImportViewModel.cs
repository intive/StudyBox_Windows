using System;
using System.Threading.Tasks;
using StudyBox.Core.Messages;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace StudyBox.Core.ViewModels
{
    public class ImageImportViewModel : ExtendedViewModelBase
    {
        private string _headerText;
        private string _deckName;
        private string _submitFormMessage;
        private string _imageName;
        private string _errorMessage;
        private bool _isPublic;
        private bool _isGeneralError;
        private bool _isDataLoading = false;
        private bool _imageNamelVisibility = false;
        private Deck _deckInstance;
        private StorageFile _image;

        private const ulong _maxImageSize = 62914560; //60 MB
        private readonly int _maxDeckNameCharacters = 50;
        private readonly int _minDeckNameCharacters = 1;

        private RelayCommand _importFileCommand;
        private RelayCommand _takePhotoCommand;
        private RelayCommand _cancel;
        private RelayCommand _submitForm;

        private ICameraService _cameraService;
        private INavigationService _navigationService;
        private IRestService _restService;
        private IInternetConnectionService _internetConnectionService;

        public ImageImportViewModel(ICameraService cameraService, INavigationService navigationService, IRestService restService, IInternetConnectionService internetConnectionService) : base(navigationService)
        {
            Messenger.Default.Register<DataMessageToCreateFlashcard>(this, x => HandleDataMessage(x.DeckInstance));
            _navigationService = navigationService;
            _cameraService = cameraService;
            _restService = restService;
            _internetConnectionService = internetConnectionService;
        }

        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                if (_headerText != value)
                {
                    _headerText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DeckName
        {
            get
            {
                return _deckName;
            }
            set
            {
                if (_deckName != value)
                {
                    _deckName = value;
                    RaisePropertyChanged("DeckName");
                    RaisePropertyChanged("CurrentDeckNameCharactersNumber");
                    RaisePropertyChanged("IsDeckNameValid");
                }
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                if(_errorMessage != value)
                {
                    _errorMessage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsPublic
        {
            get
            {
                return _isPublic;
            }
            set
            {
                if (_isPublic != value)
                {
                    _isPublic = value;
                    RaisePropertyChanged("IsPublic");
                }
            }
        }

        public int MaxDeckNameCharacters
        {
            get
            {
                return _maxDeckNameCharacters;
            }
        }

        public int CurrentDeckNameCharactersNumber
        {
            get
            {
                return DeckName.Length;
            }
        }

        public bool IsDeckNameValid
        {
            get
            {
                if (CurrentDeckNameCharactersNumber > MaxDeckNameCharacters || CurrentDeckNameCharactersNumber < _minDeckNameCharacters)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsGeneralError
        {
            get
            {
                return _isGeneralError;
            }
            set
            {
                if (_isGeneralError != value)
                {
                    _isGeneralError = value;
                    RaisePropertyChanged("IsGeneralError");
                }
            }
        }

        public string SubmitFormMessage
        {
            get
            {
                return _submitFormMessage;
            }
            set
            {
                if (_submitFormMessage != value)
                {
                    _submitFormMessage = value;
                    RaisePropertyChanged("SubmitFormMessage");
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

        public string ImageName 
        {
            get
            {
                return _imageName;
            }
            set
            {
                if(_imageName != value)
                {
                    _imageName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ImageNameVisibility
        {
            get
            {
                return _imageNamelVisibility;
            }
            set
            {
                if (_imageNamelVisibility != value)
                {
                    _imageNamelVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand ImportFileCommand
        {
            get
            {
                return _importFileCommand ?? (_importFileCommand = new RelayCommand(ImportFile));
            }
        }

        public RelayCommand TakePhotoCommand
        {
            get
            {
                return _takePhotoCommand ?? (_takePhotoCommand = new RelayCommand(TakePhoto));
            }
        }

        public RelayCommand Cancel
        {
            get
            {
                return _cancel ?? (_cancel = new RelayCommand(LeaveForm));
            }
        }

        public RelayCommand SubmitForm
        {
            get
            {
                return _submitForm ?? (_submitForm = new RelayCommand(UploadImage));
            }
        }

        private void HandleDataMessage(Deck deck)
        {
            ErrorMessage = String.Empty;
            IsGeneralError = false;
            ImageNameVisibility = false;
            ImageName = String.Empty;
            _image = null;
            _deckInstance = deck;
            if (_deckInstance == null)
            {
                HeaderText = StringResources.GetString("CreateNewDeckFromFile");
                SubmitFormMessage = StringResources.GetString("CreateNewDeck");
                DeckName = "";
                IsPublic = true;
            }
            else
            {
                HeaderText = StringResources.GetString("AddFlashcardsFromFile");
                SubmitFormMessage = StringResources.GetString("AddFlashcards");
                DeckName = deck.Name;
                IsPublic = deck.IsPublic;
            }
        }

        private async void ImportFile()
        {
            StorageFile image = await _cameraService.PickPhoto();
            if (image != null)
            {
                _image = image;
                ImageName = _image.Name;
                ImageNameVisibility = true;
            }
        }

        private async void TakePhoto()
        {
            StorageFile image;
            if (await _cameraService.IsCamera())
            {
                image = await _cameraService.CapturePhoto();
                if (image != null)
                {
                    _image = image;
                    ImageName = _image.Name;
                    ImageNameVisibility = true;
                }
            }
            else
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("CameraNotFound")));
            }

        }

        private async void UploadImage()
        {
            if (!await _internetConnectionService.IsNetworkAvailable())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("NoInternetConnection")));
                return;
            }
            else if (!_internetConnectionService.IsInternetAccess())
            {
                Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("AccessDenied")));
                return;
            }
            else
            {
                if (!ValidateForm())
                {
                    return;
                }
                if (await IsImageToLarge(_image))
                {
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("ImageTooLarge")));
                    return;
                }
                bool deckCreated = false;
                IsDataLoading = true;
                try
                {
                    //TODO komunikacja z serwerem (dodanie fiszek ze zdjęcia do nowej talii lub do już istniejącej talii na podstawie jej id)
                    throw new NotImplementedException();

                    //if (_deckInstance == null)
                    //{
                    //    _deckInstance = await _restService.CreateDeck(new Deck { Name = DeckName.Trim(), IsPublic = IsPublic });
                    //    deckCreated = true;
                    //}
                    //else
                    //{
                    //    string oldDeckName = _deckInstance.Name;
                    //    bool oldIsPublic = _deckInstance.IsPublic;
                    //    _deckInstance.Name = DeckName.Trim();
                    //    _deckInstance.IsPublic = IsPublic;
                    //    if (oldDeckName != _deckInstance.Name || oldIsPublic != _deckInstance.IsPublic)
                    //    {
                    //        await _restService.UpdateDeck(_deckInstance);
                    //    }
                    //}
                }
                catch
                {
                    //if (deckCreated)
                    //{
                    //    _deckInstance = null;
                    //    deckCreated = false;
                    //    await _restService.RemoveDeck(_deckInstance.ID);
                    //}
                    Messenger.Default.Send<MessageToMessageBoxControl>(new MessageToMessageBoxControl(true, false, StringResources.GetString("OperationFailed")));
                }

                finally
                {
                    IsDataLoading = false;
                }
            }
        }

        private async Task<bool> IsImageToLarge(StorageFile image)
        {
            BasicProperties imageProperties = await image.GetBasicPropertiesAsync();
            if (imageProperties.Size <= _maxImageSize)
                return false;
            else
                return true;
        }

        private bool ValidateForm()
        {
            if (!IsDeckNameValid || String.IsNullOrEmpty(DeckName.Trim()))
            {
                ErrorMessage = StringResources.GetString("DeckNameError");
                IsGeneralError = true;
                return false;
            }
            else if (_image == null)
            {
                ErrorMessage = StringResources.GetString("ImageError");
                IsGeneralError = true;
                return false;
            }

            IsGeneralError = false;
            return true;
        }

        private void LeaveForm()
        {
            if (_deckInstance == null)
            {
                NavigationService.NavigateTo("DecksListView");
                Messenger.Default.Send<ReloadMessageToDecksList>(new ReloadMessageToDecksList(true));
            }
            else
            {
                NavigationService.NavigateTo("ManageDeckView");
                Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(false, false));
            }
        }
    }
}
