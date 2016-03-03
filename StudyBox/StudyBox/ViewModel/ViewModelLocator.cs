using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
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
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<DecksListViewModel>();
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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

      
    }

}
