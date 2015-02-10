using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Akavache;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Phone.Controls;
using ReactiveUI;
using TestPortableLib;

namespace TestClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async Task<IEnumerable<string>> Load()
        {
            var task = BlobCache.LocalMachine.GetAllObjects<string>().ObserveOn(TaskPoolScheduler.Default);
            task.Subscribe(x =>
            {

            });
            return await task;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var task = Load();
            var results = await task;

            MessageBox.Show(results.FirstOrDefault() ?? "nothing");

        }

        private async void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            await BlobCache.LocalMachine.InsertObject("blah", "blah");
        }
    }
}