using System.Linq;
using System.ServiceModel.Syndication;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using PodcastReader.Phone8.Classes;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;

namespace PodcastReader.Phone8.Views
{
    public partial class MainView : PhoneApplicationPage, IViewFor<MainViewViewModel>
    {
        private MainViewViewModel _viewModel;

        public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var feedsProvider = new TestFeedsProvider();
            feedsProvider.GetFeeds()
                         .ObserveOnDispatcher()
                         .Subscribe(Observer.Create<SyndicationFeed>(feed => MessageBox.Show(feed.Items.Count().ToString())));
        }

        public MainViewViewModel ViewModel
        {
            get { return _viewModel; }
            set 
            { 
                _viewModel = value;
                this.DataContext = _viewModel;
            }
        }

        object IViewFor.ViewModel { get { return this.ViewModel; } set { this.ViewModel = (MainViewViewModel) value; } }
    }
}