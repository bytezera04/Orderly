
namespace Orderly.Shared.Helpers
{
    public static class DateTimeHelper
    {
        public static string ToShortTimeAgo(this DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan.TotalSeconds < 60)
                return "Now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes}m ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours}h ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays}d ago";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)}w ago";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)}m ago";

            return $"{(int)(timeSpan.TotalDays / 365)}y ago";
        }

        public static string ToReadableDateLong(DateTime date)
        {
            int day = date.Day;
            string suffix = GetDaySuffix(day);

            return $"{date:MMMM} {day}{suffix} {date:yyyy}";
        }

        public static string ToReadableDateShort(DateTime date)
        {
            int day = date.Day;
            string suffix = GetDaySuffix(day);

            return $"{date:MMM} {day}{suffix} {date:yyyy}";
        }

        private static string GetDaySuffix(int day)
        {
            if (day >= 11 && day <= 13)
                return "th";

            return (day % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        }
    }
}
