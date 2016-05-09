using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox.Core.ViewModels
{
    public class CreateDeckViewModel : ExtendedViewModelBase
    {
        private RelayCommand _createDeckWithFlashcard;
        private RelayCommand _createDeckFromFile;

        public CreateDeckViewModel(INavigationService navigationService) : base(navigationService)
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
            Messenger.Default.Send<NewDeckMessageToImageImport>(new NewDeckMessageToImageImport(true));
        }
    }
}
