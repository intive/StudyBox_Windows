using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBox.Core.Messages;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Models;
using Windows.UI.Popups;
using Windows.Storage;

namespace StudyBox.Core.ViewModels
{
    public class ImageImportViewModel : ExtendedViewModelBase
    {
        private string _headerText;
        private bool _isNewDeck;
        private RelayCommand _importFileCommand;
        private RelayCommand _takePhotoCommand;
        private ICameraService _cameraService;
        private INavigationService _navigationService;
        private IRestService _restService;

        public ImageImportViewModel(ICameraService cameraService, INavigationService navigationService, IRestService restService) : base(navigationService)
        {
            Messenger.Default.Register<NewDeckMessageToImageImport>(this, x => HandleNewDeckMessage(x.IsNewDeck));
            _navigationService = navigationService;
            _cameraService = cameraService;
            _restService = restService;
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

        private void HandleNewDeckMessage(bool isNewDeck)
        {
            _isNewDeck = isNewDeck;
            if (!isNewDeck)
                HeaderText = StringResources.GetString("CreateNewDeckFromFile");
            else
                HeaderText = StringResources.GetString("AddFlashcardsFromFile");
        }

        private async void ImportFile()
        {
            StorageFile image = await _cameraService.PickPhoto();
            if (image != null)
                UploadImage(image);
        }

        private async void TakePhoto()
        {
            StorageFile image;
            if (await _cameraService.IsCamera())
            {
                image = await _cameraService.CapturePhoto();
                if (image != null)
                    UploadImage(image);
            }                
            else
            {
                MessageDialog msg = new MessageDialog(StringResources.GetString("CameraNotFound"));
                await msg.ShowAsync();
            }

        }

        private async void UploadImage(StorageFile image)
        {
            if (_isNewDeck)
            {
               Deck deck = await _restService.CreateDeck(new Deck { Name = image.DisplayName, IsPublic = true });
            }

            //TODO komunikacja z serwerem (dodanie fiszek ze zdjęcia do nowej talii lub do już istniejącej talii na podstawie jej id)
        }
    }
}
