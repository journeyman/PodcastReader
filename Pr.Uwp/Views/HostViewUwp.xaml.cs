using Pr.Core.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace Pr.Uwp.Views
{
	public sealed partial class HostViewUwp : WindowsPage
	{
		public HostViewUwp()
		{
			this.InitializeComponent();

			LoadApplication(new PrApp());
		}
	}
}
