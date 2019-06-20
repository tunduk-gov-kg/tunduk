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
        /// <param name="conversionType">type of conversion</param>
        /// <returns>converted DateTime value</returns>
        /// <exception cref="ArgumentOutOfRangeException">in case of unknown conversionType was given</exception>
        public static DateTime ToDateTime(this long value, ConversionType conversionType)
        {
            var timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            switch (conversionType)
            {
                case ConversionType.Milliseconds:
                    return timestamp.AddMilliseconds(value).ToLocalTime();
                case ConversionType.Seconds:
                    return timestamp.AddSeconds(value).ToLocalTime();
                default:
                    throw new ArgumentOutOfRangeException(nameof(conversionType), conversionType, null);
            }
        }
    }

    public enum ConversionType
    {
        Seconds,
        Milliseconds
    }
}