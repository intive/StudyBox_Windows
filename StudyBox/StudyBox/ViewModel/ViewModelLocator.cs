using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using StudyBox.View;
using GalaSoft.MvvmLight;
using StudyBox.Interfaces;
using StudyBox.Services;

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
            SimpleIoc.Default.Register<ExamViewModel>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDeserializeJsonService, DeserializeJsonService>();
                SimpleIoc.Default.Register<IRestService, RestService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDeserializeJsonService, DeserializeJsonService>();
                SimpleIoc.Default.Register<IRestService, RestService>();
            }
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
