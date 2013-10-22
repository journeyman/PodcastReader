using System.Threading.Tasks;
using Akavache;
using Microsoft.Phone.Shell;
using PodcastReader.Phone8.ViewModels;
using ReactiveUI;

namespace PodcastReader.Phone8.Infrastructure
{
    public class AppLifetime
    {
        public async void OnLaunching()
        {
            await InitApp();
            RunInitedApp();
        }

        public async void OnActivated(bool statePreserved)
        {
            if (!statePreserved)
                await InitApp();
            RunInitedApp();
        }

        public async void OnDeactivated(DeactivationReason reason)
        {
            await SaveAppState();
        }

        public async void OnClosing()
        {
            await SaveAppState();
        }

        private async Task InitApp()
        {
            var b = new AppBootstrapper(); //IoC registrations
            
            BlobCache.ApplicationName = "PodcastReader";
            BlobCache.EnsureInitialized();
        }

        private void RunInitedApp()
        {
            Screen.Router.Navigate.Execute(RxApp.DependencyResolver.GetService<MainViewModel>());
        }

        private async Task SaveAppState()
        {
            await BlobCache.Shutdown();
        }
    }
}
