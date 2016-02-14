using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Pr.Core
{
	public partial class NavigationHostView : NavigationPage
	{
		public NavigationHostView(Page root) : base(root)
		{
			InitializeComponent();
		}
	}
}
