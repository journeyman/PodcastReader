using System;
using System.Globalization;
using System.Windows.Data;

namespace Pr.Phone8.Infrastructure.Converters
{
    public abstract class ConverterBase<TIn, TOut> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertSafe((TIn) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public abstract TOut ConvertSafe(TIn value);
    }
}
