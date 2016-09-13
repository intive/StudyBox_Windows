using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using StudyBox.View;
using StudyBox.Core.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using StudyBox.Core.Messages;
using StudyBox.Core.Enums;
using Windows.System.Profile;

namespace StudyBox
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        static string deviceFamily;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
            //Change the pointer mode to support selected mode rather than pointer mode
            App.Current.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = false;
            }
#endif


            if (App.IsXbox)
            {
                // use TV colorsafe values
                this.Resources.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("ms-appx:///Resources/Themes/TvSafeColors.xaml")
                });

                // remove TV Safe areas
                ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            }

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                IAccountService service = SimpleIoc.Default.GetInstance<IAccountService>();
                if (service.IsUserLoggedIn())
                {
                    rootFrame.Navigate(typeof(DecksListView), e.Arguments);
                    Messenger.Default.Send<DecksTypeMessage>(new DecksTypeMessage(DecksType.MyDecks));
                }
                else
                {
                    rootFrame.Navigate(typeof(LoginView), e.Arguments);
                }
            }

            Window.Current.Activate();
        }
        public static event EventHandler IsXboxModeChanged;
        //public static bool IsXbox()
        //{

        //    if (deviceFamily == null)
        //        deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
        //    return true;
        //    return deviceFamily == "Windows.Xbox";

        //}

        public static bool IsXbox
        {
            get
            {
                return true;
                //return AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox" || IsXboxPC;
            }
            set
            {
                if (value != IsXboxPC)
                {
                    IsXboxPC = value;
                    IsXboxModeChanged?.Invoke(null, null);
                }
            }
        }

        public static bool IsXboxPC { get; private set; } = IsXbox;
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
