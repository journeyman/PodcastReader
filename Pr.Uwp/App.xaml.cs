using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Pr.Uwp.Infrastructure;
using Pr.Uwp.Views;

namespace Pr.Uwp
{
    public sealed partial class App : Application
    {
        private PrApp _app;

        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;
        }

	    public static Frame RootFrame => (Frame)Window.Current.Content;

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                var hostview = new HostView();
                rootFrame.Content = hostview;
                _app = new PrApp(hostview);
                //rootFrame.Navigate(typeof(HostView), e.Arguments);
            }

            Window.Current.Activate();

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated
                || e.PreviousExecutionState == ApplicationExecutionState.NotRunning
                || e.PreviousExecutionState == ApplicationExecutionState.ClosedByUser)
            {
                _app.OnStart();
            }
            else
            {
                _app.OnResume();
            }
        }

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();

            _app.OnSleep();
        }
    }
}
