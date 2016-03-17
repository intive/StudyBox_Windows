using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Windows.ApplicationModel.Resources;

namespace StudyBox.Core.ViewModels
{
    public class ExtendedViewModelBase : ViewModelBase
    {
        private INavigationService _navigationService;
        private ResourceLoader _stringResources;

        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set { _navigationService = value; }
        }

        public ResourceLoader StringResources
        {
            get { return _stringResources; }
            set { _stringResources = value; }
        }

        public ExtendedViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            StringResources = new ResourceLoader();
        }
    }
}
