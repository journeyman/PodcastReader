using System;

namespace Pr.Core.Utils
{
    public static class DateTimeExtensions
    {
        public static string ToTimeAgo(this TimeSpan timeSince)
        {
            if (timeSince.TotalMinutes < 1)
                return "now";
            if (timeSince.TotalMinutes < 60)
                return string.Format("{0}m", timeSince.Minutes);
            if (timeSince.TotalHours < 24)
                return string.Format("{0}h", timeSince.Hours);
            if (timeSince.TotalDays < 2)
                return "yesterday";
            if (timeSince.TotalDays < 7)
                return string.Format("{0}d", timeSince.Days);
            if (timeSince.TotalDays < 14)
                return "week";
            if (timeSince.TotalDays < 21)
                return "2w";
            if (timeSince.TotalDays < 28)
                return "3w";
            if (timeSince.TotalDays < 60)
                return "last month";
            if (timeSince.TotalDays < 365)
                return string.Format("{0}month", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730)
                return "last year";

            //last but not least...
            return string.Format("{0}y", Math.Round(timeSince.TotalDays / 365));
        }
    }
}
