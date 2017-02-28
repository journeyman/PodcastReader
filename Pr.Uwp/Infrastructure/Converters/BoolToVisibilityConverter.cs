using Windows.UI.Xaml;
using Pr.Uwp.UI.Converters;

namespace Pr.Uwp.Infrastructure.Converters
{
	public class BoolToVisibilityConverterUwp : ConverterBase<bool, Visibility>
    {
        public override Visibility ConvertCore(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}