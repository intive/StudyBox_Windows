using GalaSoft.MvvmLight.Views;
using StudyBox.Core.Interfaces;

namespace StudyBox.Core.ViewModels
{
    public class MainPageViewModel : ExtendedViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, IDetectKeysService detectKeysService) : base(navigationService, detectKeysService)
        {
        }


    }
}
