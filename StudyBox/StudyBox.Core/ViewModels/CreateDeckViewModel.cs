using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using StudyBox.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace StudyBox.Core.ViewModels
{
    public class CreateDeckViewModel : ExtendedViewModelBase
    {
        private RelayCommand _createDeckWithFlashcard;
        private RelayCommand _createDeckFromFile;
        private RelayCommand<object> _gotFocusCommand;
        private RelayCommand<KeyRoutedEventArgs> _detectKeyDownCommand;

        public RelayCommand<KeyRoutedEventArgs> DetectKeyDownCommand
        {
            get { return _detectKeyDownCommand ?? (_detectKeyDownCommand = new RelayCommand<KeyRoutedEventArgs>(DetectKeysService.DetectKeyDown)); }
        }
        public RelayCommand<object> GotFocusCommand
        {
            get { return _gotFocusCommand ?? (_gotFocusCommand = new RelayCommand<object>(DetectKeysService.GotFocus)); }
        }

        public CreateDeckViewModel(INavigationService navigationService, IDetectKeysService detectKeysService) : base(navigationService, detectKeysService)
        {
        }
     
        public RelayCommand CreateDeckWithFlashcard
        {
            get
            {
                return _createDeckWithFlashcard ?? (_createDeckWithFlashcard = new RelayCommand(GoToCreateDeckWithFlashcard));
            }
        }

        public RelayCommand CreateDeckFromFile
        {
            get
            {
                return _createDeckFromFile ?? (_createDeckFromFile = new RelayCommand(GoToCreateDeckFromFile));
            }
        }

        private void GoToCreateDeckWithFlashcard()
        {
            NavigationService.NavigateTo("CreateFlashcardView");
            Messenger.Default.Send<DataMessageToCreateFlashcard>(new DataMessageToCreateFlashcard(null, null));
        }

        private void GoToCreateDeckFromFile()
        {
            NavigationService.NavigateTo("ImageImportView");
            Messenger.Default.Send<DataMessageToCreateFlashcard>(new DataMessageToCreateFlashcard(null, null));
        }
    }
}
