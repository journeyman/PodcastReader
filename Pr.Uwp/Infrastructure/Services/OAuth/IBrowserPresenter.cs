using System;
using System.Threading.Tasks;

namespace Pr.Uwp.Infrastructure.Services.OAuth
{
    public interface IBrowserPresenter
    {
        Task Navigate(Uri uri);

        event Action<Uri> Navigated;
    }
}
