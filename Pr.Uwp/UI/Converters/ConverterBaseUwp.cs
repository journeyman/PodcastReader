using System;
using Windows.UI.Xaml.Data;
using Pr.Ui.Converters;

namespace Pr.Uwp.UI.Converters
{
	public abstract class ConverterBaseUwp<TIn, TOut> : ConverterBase<TIn, TOut>, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ConvertSafe((TIn)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
            throw new NotImplementedException();
		}
	}
}
