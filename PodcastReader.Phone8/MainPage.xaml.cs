using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Shell;
using PodcastReader.Phone8.Resources;
using System.ServiceModel.Syndication;
using PodcastReader.Phone8.Classes;
using ReactiveUI;

namespace PodcastReader.Phone8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            
        }

        //private class Observer

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var feedsProvider = new TestFeedsProvider();
            feedsProvider.GetFeeds()
                         .ObserveOnDispatcher()
                         .Subscribe(Observer.Create<SyndicationFeed>(feed => MessageBox.Show(feed.Items.Count().ToString())));
        }
    }
}