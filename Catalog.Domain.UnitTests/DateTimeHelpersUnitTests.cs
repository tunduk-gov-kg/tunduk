using System;
using System.Globalization;
using Catalog.Domain.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Domain.UnitTests
{
    public class DateTimeHelpersUnitTests
    {
        private ITestOutputHelper _helper;

        public DateTimeHelpersUnitTests(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public void ToDateTime_WhenDateTime()
        {
            var dateTime = new DateTime(2000, 1, 1, 0, 0, 0);
            Assert.Equal(dateTime, 946684800L.AsSecondsToDateTime());
        }

        [Fact]
        public void ToUnixTimestamp_WhenUnixTimestamp1970_ShouldReturnZero()
        {
            var unixBegin = new DateTime(1970, 1, 1, 0, 0, 0);
            var unixTimestamp = unixBegin.ToUnixTimestamp();
            Assert.Equal(0L, unixTimestamp);
        }

        [Fact]
        public void ToUnixTimestamp_WhenUnixTimestamp2000_ShouldReturn946684800()
        {
            var unixBegin = new DateTime(2019, 5, 8, 16, 38, 6);
            var unixTimestamp = unixBegin.ToUnixTimestamp();
            _helper.WriteLine(unixTimestamp.ToString());

            var dateTime = unixTimestamp.AsSecondsToDateTime();
            _helper.WriteLine(dateTime.ToString(CultureInfo.CurrentCulture));

            var ms = unixTimestamp * 1000;
            var milliSecondsToDateTime = ms.AsMilliSecondsToDateTime();
            _helper.WriteLine(milliSecondsToDateTime.ToString(CultureInfo.CurrentCulture));
        }
    }
}