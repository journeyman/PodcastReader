using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Akavache;
using System.Linq;
using System.Reactive.Linq;
using Windows.Storage;
using Microsoft.Phone.Controls;
using TestPortableLib;

namespace TestClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("test", CreationCollisionOption.ReplaceExisting);
        }

        private async void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "test");
            Debug.WriteLine(path);
            var file = await StorageFile.GetFileFromPathAsync(path);
        }
    }
}