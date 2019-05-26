using System;

namespace Monitor.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long) dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime AsSecondsToDateTime(this long unixTimestamp)
        {
            var unixTimestampBegin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return unixTimestampBegin.AddSeconds(unixTimestamp).ToLocalTime();
        }

        public static DateTime AsMilliSecondsToDateTime(this long milliseconds)
        {
            var unixTimestampBegin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return unixTimestampBegin.AddMilliseconds(milliseconds).ToLocalTime();
        }
    }
}