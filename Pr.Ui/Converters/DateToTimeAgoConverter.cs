using System;
using Pr.Core.Utils;

namespace Pr.Ui.Converters
{
    public class DateToTimeAgoConverter : ConverterBase<DateTimeOffset, string>
	{
	    public override string ConvertSafe(DateTimeOffset date)
	    {
			var timeAgo = DateTimeOffset.Now - date;
			var str = timeAgo.ToTimeAgo();
			return str;
		}
	}
}
