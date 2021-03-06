﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Info;
using ReactiveUI;
using System.Windows.Threading;
using System.Windows.Media;
using Splat;

namespace PodcastReader.Phone8.Utils
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

        static void Timer_OnTick(object sender, EventArgs e)
        {
            string memoryString = string.Format("current: {0}, peak: {1}, limit: {2}",
                _memoryInfo.Usage.ToPrettyMbString(),
                _memoryInfo.Peak.ToPrettyMbString(),
                _memoryInfo.Limit.ToPrettyMbString());

            _instance.SetValue(ContentProperty, memoryString);
        }
    }

    public class MemoryInfo
    {
        public long Usage { get { return DeviceStatus.ApplicationCurrentMemoryUsage; } }
        public long Peak { get { return DeviceStatus.ApplicationPeakMemoryUsage; } }
        public long Limit { get { return DeviceStatus.ApplicationMemoryUsageLimit; } }
    }

    public static class LongExtensions
    {
        public static string ToPrettyMbString(this long num)
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
