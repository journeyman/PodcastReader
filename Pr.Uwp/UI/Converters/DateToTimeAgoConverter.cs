using System;
using Pr.Core.Utils;

namespace Pr.Uwp.UI.Converters
{
    public class DateToTimeAgoConverter : ConverterBase<DateTimeOffset, string>
	{
	    public override string ConvertCore(DateTimeOffset value)
	    {
            var timeAgo = DateTimeOffset.Now - value;
            var str = timeAgo.ToTimeAgo();
            return str;
        }
	}
}
