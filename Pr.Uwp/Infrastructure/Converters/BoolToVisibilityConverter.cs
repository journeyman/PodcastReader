using Windows.UI.Xaml;
using Pr.Uwp.UI.Converters;

namespace Pr.Phone8.Infrastructure.Converters
{
	public class BoolToVisibilityConverterUwp : ConverterBaseUwp<bool, Visibility>
    {
        public override Visibility ConvertSafe(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}