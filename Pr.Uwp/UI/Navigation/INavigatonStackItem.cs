using Windows.UI.Xaml;

namespace Pr.Uwp.UI.Navigation
{
    public interface INavigatonStackItem
    {
        UIElement Content { get; }
    }
}