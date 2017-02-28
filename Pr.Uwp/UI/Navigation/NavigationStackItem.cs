using Windows.UI.Xaml;

namespace Pr.Uwp.UI.Navigation
{
    public class NavigationStackItem : INavigatonStackItem
    {
        public UIElement Content { get; set; }

        public NavigationStackItem(UIElement content)
        {
            Content = content;
        }
    }
}