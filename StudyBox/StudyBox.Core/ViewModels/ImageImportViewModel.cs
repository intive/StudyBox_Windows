using System;
using System.Threading.Tasks;
using StudyBox.Core.Messages;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace StudyBox.Core.ViewModels
{
    public class ImageImportViewModel : ExtendedViewModelBase
    {
        private string _headerText;
        private string _deckName;
        private string _submitFormMessage;
        private bool _isPublic;
        private bool _isGeneralError;
        private bool _isDataLoading = false;
        private bool _thumbnailVisibility = false;
        private Deck _deckInstance;
        private StorageItemThumbnail _thumbnail;
        private StorageFile _image;

        private const ulong _maxImageSize = 62914560; //60 MB
        private readonly int _maxDeckNameCharacters = 50;

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
                if (CurrentDeckNameCharactersNumber > MaxDeckNameCharacters || CurrentDeckNameCharactersNumber == 0)
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

        public StorageItemThumbnail Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                if(_thumbnail != value)
                {
                    _thumbnail = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ThumbnailVisibility
        {
            get
            {
                return _thumbnailVisibility;
            }
            set
            {
                if (_thumbnailVisibility != value)
                {
                    _thumbnailVisibility = value;
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
            ThumbnailVisibility = false;
            Thumbnail = null;
            _image = null;
            if (deck == null)
            {
                HeaderText = StringResources.GetString("CreateNewDeckFromFile");
                SubmitFormMessage = StringResources.GetString("CreateNewDeck");
                DeckName = "";
            }
            else
            {
                HeaderText = StringResources.GetString("AddFlashcardsFromFile");
                SubmitFormMessage = StringResources.GetString("AddFlashcard");
                DeckName = deck.Name;
                _deckInstance = deck;
            }
        }

        private async void ImportFile()
        {
            StorageFile image = await _cameraService.PickPhoto();
            if (image != null)
            {
                _image = image;
                Thumbnail = await image.GetThumbnailAsync(ThumbnailMode.PicturesView);
                ThumbnailVisibility = true;
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
                    Thumbnail = await image.GetThumbnailAsync(ThumbnailMode.PicturesView);
                    ThumbnailVisibility = true;
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
                MessageDialog msg = new MessageDialog((StringResources.GetString("NoInternetConnection")));
                await msg.ShowAsync();
                return;
            }
            else if (!_internetConnectionService.IsInternetAccess())
            {
                MessageDialog msg = new MessageDialog((StringResources.GetString("AccessDenied")));
                await msg.ShowAsync();
                return;
            }
            else
            {
                if (!ValidateForm())
                {
                    return;
                }
                if (_image != null)
                {
                    if (await IsImageToLarge(_image))
                    {
                        MessageDialog msg = new MessageDialog(StringResources.GetString("ImageTooLarge"));
                        await msg.ShowAsync();
                        return;
                    }

                    bool deckCreated = false;
                    IsDataLoading = true;
                    try
                    {
                        if (_deckInstance == null)
                        {
                            _deckInstance = await _restService.CreateDeck(new Deck { Name = DeckName.Trim(), IsPublic = IsPublic });
                            deckCreated = true;
                        }
                        else
                        {
                            string oldDeckName = _deckInstance.Name;
                            bool oldIsPublic = _deckInstance.IsPublic;
                            _deckInstance.Name = DeckName.Trim();
                            _deckInstance.IsPublic = IsPublic;
                            if (oldDeckName != _deckInstance.Name || oldIsPublic != _deckInstance.IsPublic)
                            {
                                await _restService.UpdateDeck(_deckInstance);
                            }
                        }

                        try
                        {
                            //TODO komunikacja z serwerem (dodanie fiszek ze zdjęcia do nowej talii lub do już istniejącej talii na podstawie jej id)
                            throw new NotImplementedException();
                        }
                        catch
                        {
                            if (deckCreated)
                                await _restService.RemoveDeck(_deckInstance.ID);
                            MessageDialog msg = new MessageDialog(StringResources.GetString("OperationFailed"));
                            await msg.ShowAsync();
                        }
                    }
                    catch
                    {
                        MessageDialog msg = new MessageDialog(StringResources.GetString("OperationFailed"));
                        await msg.ShowAsync();
                    }
                    finally
                    {
                        IsDataLoading = false;
                    }
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
            if (!IsDeckNameValid || _image == null)
            {
                IsGeneralError = true;
                return false;
            }

            IsGeneralError = false;
            return true;
        }

        private void LeaveForm()
        {
            NavigationService.NavigateTo("DecksListView");
            Messenger.Default.Send<ReloadMessageToDecksList>(new ReloadMessageToDecksList(true));
            Messenger.Default.Send<MessageToMenuControl>(new MessageToMenuControl(true, false, false));
        }
    }
}
