using System;
using System.Globalization;
using Catalog.Domain.Entity;
using Catalog.Domain.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Domain.UnitTests
{
    public class OperationalDataRecordHelpersTests
    {
        public OperationalDataRecordHelpersTests(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        private readonly ITestOutputHelper _helper;

        [Fact]
        public void CalculateDigest()
        {
            var operationalDataRecord = new OperationalDataRecord
            {
                ClientMemberClass = "GOV",
                ClientMemberCode = "100",
                ClientSecurityServerAddress = "127.0.0.1",
                ClientSubsystemCode = "SSSS",
                ClientXRoadInstance = "KG",
                MessageId = "MessageID"
            };
            var calculateDigest = operationalDataRecord.CalculateDigest();

            Assert.NotNull(calculateDigest);
        }

        [Fact]
        public void CalculateDigest_MultipleTimes()
        {
            var begin = DateTime.Now;
            for (var i = 0; i < 100000; i++)
            {
                var operationalDataRecord = new OperationalDataRecord
                {
                    ClientMemberClass = "GOV",
                    ClientMemberCode = i.ToString(),
                    ClientSecurityServerAddress = "127.0.0.1",
                    ClientSubsystemCode = "Client",
                    ClientXRoadInstance = "KG",
                    MessageId = "MessageID"
                };
                var calculateDigest = operationalDataRecord.CalculateDigest();
            }

            var end = DateTime.Now;

            var timeSpan = end - begin;
            _helper.WriteLine(timeSpan.TotalSeconds.ToString(CultureInfo.InvariantCulture));
        }
    }
}