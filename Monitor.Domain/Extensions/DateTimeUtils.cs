using System;

namespace Monitor.Domain.Extensions
{
    public static class DateTimeUtils
    {
        /// <summary>
        ///     Converts given datetime to unix timestamp seconds
        /// </summary>
        /// <param name="dateTime">target argument for conversion</param>
        /// <returns></returns>
        public static long ToSeconds(this DateTime dateTime)
        {
            return (long) dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        ///     Converts given unix timestamp to DateTime
        /// </summary>
        /// <param name="value">unix timestamp for conversion</param>
        /// <param name="temporalType">type of conversion</param>
        /// <returns>converted DateTime value</returns>
        /// <exception cref="ArgumentOutOfRangeException">in case of unknown conversionType was given</exception>
        public static DateTime ToDateTime(this long value, TemporalType temporalType)
        {
            var timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            switch (temporalType)
            {
                case TemporalType.Milliseconds:
                    return timestamp.AddMilliseconds(value).ToLocalTime();
                case TemporalType.Seconds:
                    return timestamp.AddSeconds(value).ToLocalTime();
                default:
                    throw new ArgumentOutOfRangeException(nameof(temporalType), temporalType, null);
            }
        }
    }

    public enum TemporalType
    {
        Seconds,
        Milliseconds
    }
}