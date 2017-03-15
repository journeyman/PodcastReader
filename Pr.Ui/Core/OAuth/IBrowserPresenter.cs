using System;
using System.Threading.Tasks;

namespace Pr.Ui.Core.OAuth
{
    public interface IBrowserPresenter
    {
        Task Navigate(Uri uri);

        event Action<Uri> Navigated;
    }
}
