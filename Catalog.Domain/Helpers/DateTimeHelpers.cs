using System;

namespace Catalog.Domain.Helpers
{
    public static class DateTimeHelpers
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long) dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime AsSecondsToDateTime(this long unixTimestamp)
        {
            DateTime unixTimestampBegin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return unixTimestampBegin.AddSeconds(unixTimestamp).ToLocalTime();
        }

        public static DateTime AsMilliSecondsToDateTime(this long milliseconds)
        {
            DateTime unixTimestampBegin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return unixTimestampBegin.AddMilliseconds(milliseconds).ToLocalTime();
        }
    }
}