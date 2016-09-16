using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;
using Windows.ApplicationModel.Resources;

namespace StudyBox.Core.ViewModels
{
    public class ExtendedViewModelBase : ViewModelBase
    {
        private INavigationService _navigationService;
        private ResourceLoader _stringResources;
        private IDetectKeysService _detectKeysService;

        public INavigationService NavigationService
        {
            get { return _navigationService; }
            set { _navigationService = value; }
        }

        public IDetectKeysService DetectKeysService
        {
            get { return _detectKeysService; }
            set { _detectKeysService = value; }
        }

        public ResourceLoader StringResources
        {
            get { return _stringResources; }
            set { _stringResources = value; }
        }

        public ExtendedViewModelBase(INavigationService navigationService, IDetectKeysService detectKeysService)
        {
            NavigationService = navigationService;
            DetectKeysService = detectKeysService;
            StringResources = new ResourceLoader();
        }
    }
}
