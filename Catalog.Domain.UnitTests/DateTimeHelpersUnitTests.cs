using System;
using Catalog.Domain.Helpers;
using Xunit;

namespace Catalog.Domain.UnitTests
{
    public class DateTimeHelpersUnitTests
    {
        [Fact]
        public void ToUnixTimestamp_WhenUnixTimestamp1970_ShouldReturnZero()
        {
            var unixBegin     = new DateTime(1970, 1, 1, 0, 0, 0);
            var unixTimestamp = unixBegin.ToUnixTimestamp();
            Assert.Equal(0L, unixTimestamp);
        }

        [Fact]
        public void ToUnixTimestamp_WhenUnixTimestamp2000_ShouldReturn946684800()
        {
            var unixBegin     = new DateTime(2000, 1, 1, 0, 0, 0);
            var unixTimestamp = unixBegin.ToUnixTimestamp();
            Assert.Equal(946684800L, unixTimestamp);
        }

        [Fact]
        public void ToDateTime_WhenDateTime()
        {
            var dateTime = new DateTime(2000, 1, 1, 0, 0, 0);
            Assert.Equal(dateTime, 946684800L.ToDateTime());
        }
    }
}