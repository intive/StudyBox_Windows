using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using StudyBox.View;

namespace StudyBox.ViewModel
{
    public class ViewModelLocator
    {
         /// <summary>
         /// Initializes a new instance of the ViewModelLocator class.
         /// </summary>
        public ViewModelLocator()
         {
             var navigationService = CreateNavigationService();
            SimpleIoc.Default.Register<INavigationService>(()=>navigationService);

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<DecksListViewModel>();
            SimpleIoc.Default.Register<ExamViewModel>();
        }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("DecksListView",typeof(DecksListView));
            navigationService.Configure("ExamView",typeof(ExamView));
            navigationService.Configure("MainPage",typeof(MainPage));

            return navigationService;
        }

        public MainPageViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainPageViewModel>();
            }
        }

        public DecksListViewModel DecksViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DecksListViewModel>();
            }
        }

        public ExamViewModel ExamsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ExamViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

      
    }

}
