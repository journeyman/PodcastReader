using System.Windows;
using Windows.UI.Xaml;

namespace Pr.Phone8.Infrastructure.Converters
{
	public class BoolToVisibilityConverter : ConverterBase<bool, Visibility>
    {
        public override Visibility ConvertSafe(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}