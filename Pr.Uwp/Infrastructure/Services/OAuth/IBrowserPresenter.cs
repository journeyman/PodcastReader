using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Pr.Uwp.Infrastructure.Services.OAuth
{
    public interface IBrowserPresenter
    {
        Task Navigate(Uri uri);
        void Close();

        IObservable<Uri> Navigating { get; }

        IObservable<Unit> Closed { get; }
    }
}
