using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

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
        }


        public MainPageViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainPageViewModel>();
            }
        }



        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

      
    }

}
