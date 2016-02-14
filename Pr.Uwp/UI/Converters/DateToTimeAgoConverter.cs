using System;
using System.Globalization;
using System.Windows.Data;
using Pr.Core.Utils;

namespace Pr.Phone8.Ui.Converters
{
    public class DateToTimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTimeOffset)value;
            var timeAgo = DateTime.Now - date;
            var str = timeAgo.ToTimeAgo();
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
