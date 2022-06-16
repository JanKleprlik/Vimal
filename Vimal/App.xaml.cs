using System;

using Vimal.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;

namespace Vimal
{
    public sealed partial class App : Application
    {
        public static BackgroundTaskDeferral AppServiceDeferral = null;
        public static AppServiceConnection Connection = null;
        public static event EventHandler AppServiceDisconnected;
        public static event EventHandler<AppServiceTriggerDetails> AppServiceConnected;
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;

            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }

            //Set Theme
            await ThemeSelectorService.SetThemeAsync(ElementTheme.Dark);

            //Set top right corner buttons visuals
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

            titleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;

            var transparentColor = Windows.UI.Colors.Transparent;
            var lessTransparentColor = Windows.UI.Colors.Transparent;
            lessTransparentColor.A = 128;

            titleBar.ButtonBackgroundColor = transparentColor;
            titleBar.ButtonHoverBackgroundColor = lessTransparentColor;
            titleBar.ButtonPressedBackgroundColor = lessTransparentColor;
            titleBar.ButtonInactiveBackgroundColor = transparentColor;
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);

            //Handle messaging in the background
            if (args.TaskInstance.TriggerDetails is AppServiceTriggerDetails details)
            {
                // only accept connections from callers in the same package
                if (details.CallerPackageFamilyName == Package.Current.Id.FamilyName)
                {
                    // connection established from the fulltrust process
                    AppServiceDeferral = args.TaskInstance.GetDeferral();
                    args.TaskInstance.Canceled += OnTaskCanceled;

                    Connection = details.AppServiceConnection;
                    AppServiceConnected?.Invoke(this, args.TaskInstance.TriggerDetails as AppServiceTriggerDetails);
                }
            }
        }

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage));
        }

        /// <summary>
        /// Task canceled here means the app service client is gone
        /// </summary>
        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            AppServiceDeferral?.Complete();
            AppServiceDeferral = null;
            Connection = null;
            AppServiceDisconnected?.Invoke(this, null);
        }
    }
}
