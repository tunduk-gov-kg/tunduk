using System.Threading.Tasks;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class XRoadManagerUnitTests
    {
        [Fact]
        public async Task GetServicesListAsync_When__()
        {
            var xRoadManager = XRoadManagerProvider.RequireXRoadManager();
            var servicesListAsync = await xRoadManager.GetServicesListAsync();
            Assert.True(servicesListAsync.Count > 0);
        }

        [Fact]
        public async Task GetMembersListAsync_When__()
        {
            var xRoadManager = XRoadManagerProvider.RequireXRoadManager();
            var immutableList = await xRoadManager.GetMembersListAsync();
            Assert.True(immutableList.Count > 0);
        }

        [Fact]
        public async Task GetSecurityServersListAsync_When__()
        {
            var xRoadManager = XRoadManagerProvider.RequireXRoadManager();
            var immutableList = await xRoadManager.GetSecurityServersListAsync();
            Assert.True(immutableList.Count > 0);
        }

        [Fact]
        public async Task GetSubSystemsListAsync_When__()
        {
            var xRoadManager = XRoadManagerProvider.RequireXRoadManager();
            var immutableList = await xRoadManager.GetSubSystemsListAsync();
            Assert.True(immutableList.Count > 0);
        }
    }
}