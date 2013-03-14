using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ReactiveUI;
using System.Windows.Threading;
using System.Windows.Media;

namespace PodcastReader.Phone8.Utils
{
    public class MemoryPanel : ContentControl
    {
        private static MemoryPanel _instance;
        private static TimeSpan _updatePeriod;

        public static void Show(TimeSpan updatePeriod)
        {
            if (_instance != null)
                return;

            _updatePeriod = updatePeriod;
            App.RootFrame.Navigated += RootFrame_OnNavigated;
        }

        private static void RootFrame_OnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            App.RootFrame.Navigated -= RootFrame_OnNavigated;
            var content = (FrameworkElement)App.RootFrame.Content;
            var panel = content.GetVisualChildren<Panel>().FirstOrDefault();
            if (panel == null)
            {
                RxApp.LoggerFactory(typeof(MemoryPanel)).Write("Failed to init MemoryPanel: containing panel not found!", LogLevel.Debug);
                return;
            }
            
            _instance = new MemoryPanel()
                            {
                                Opacity = 70d,
                                VerticalAlignment = VerticalAlignment.Bottom,
                                HorizontalAlignment = HorizontalAlignment.Right,
                                Foreground = new SolidColorBrush(Colors.Green)
                            };

            panel.Children.Add(_instance);

            var timer = new DispatcherTimer {Interval = _updatePeriod};
            timer.Tick += Timer_OnTick;
            timer.Start();
        }

        static void Timer_OnTick(object sender, EventArgs e)
        {
            string memoryString = string.Format("current: {0}, peak: {1}, limit: {2}", 10, 20, 30);
            _instance.SetValue(ContentProperty, memoryString);
        }
    }
}
