using System.Windows;
using System.Windows.Navigation;
using Akavache;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Phone.Controls;

namespace TestClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        private const string KEY = "key";

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            txtTest.Text = (await BlobCache.UserAccount.GetAllObjects<string>()).FirstOrDefault() ?? string.Empty;
        }

        private async void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            await BlobCache.UserAccount.InsertObject(KEY, txtTest.Text);
        }
    }
}