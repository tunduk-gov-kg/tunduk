using System;

namespace Catalog.Domain.Helpers
{
    public static class DateTimeHelpers
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long) dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime ToDateTime(this long unixTimestamp)
        {
            DateTime unixTimestampBegin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return unixTimestampBegin.AddSeconds(unixTimestamp).ToLocalTime();
        }
    }
}