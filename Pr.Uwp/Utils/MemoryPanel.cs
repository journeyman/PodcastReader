using System;
using System.Linq;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Pr.Uwp;
using Splat;

namespace Pr.Phone8.Utils
{
    public class MemoryPanel : ContentControl
    {
        private static MemoryPanel _instance;
        private static TimeSpan _updatePeriod;
        private static MemoryInfo _memoryInfo;

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
                LogHost.Default.Debug("Failed to init MemoryPanel: containing panel not found!");
                return;
            }
            
            _instance = new MemoryPanel()
                            {
                                Opacity = 70d,
                                VerticalAlignment = VerticalAlignment.Bottom,
                                HorizontalAlignment = HorizontalAlignment.Right,
                                Foreground = new SolidColorBrush(Colors.Green),
                                FontSize = 14,
                                IsHitTestVisible = false,
                                FontWeight = FontWeights.SemiBold,
                            };

            Grid.SetColumnSpan(_instance, 100);
            Grid.SetRowSpan(_instance, 100);

            panel.Children.Add(_instance);

            _memoryInfo = new MemoryInfo();

            var timer = new DispatcherTimer {Interval = _updatePeriod};
            timer.Tick += Timer_OnTick;
            timer.Start();
        }

        static void Timer_OnTick(object sender, object e)
        {
            var memoryString = $"cur: {_memoryInfo.Usage.ToPrettyMbString()}, cap: {_memoryInfo.Limit.ToPrettyMbString()}";
	        _instance.Content = memoryString;
        }
    }

    public class MemoryInfo
    {
        public ulong Usage => MemoryManager.AppMemoryUsage;
	    public ulong Limit => MemoryManager.AppMemoryUsageLimit;
    }

    public static class LongExtensions
    {
        public static string ToPrettyMbString(this long num)
        {
            return ((ulong)num).ToPrettyMbString();
        }

		public static string ToPrettyMbString(this ulong num)
		{
			if (num <= 1024)
				return num.ToString() + " b";
			else if (num <= 1024 * 1024)
				return (num / 1024).ToString() + " Kb";
			else
				return (num / (1024 * 1024)).ToString("N2") + " Mb";
		}
	}
}
