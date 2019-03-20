using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.Service.XRoad;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests {
    public class UpdateManagerUnitTests {
        [Fact]
        public async Task RunBatchUpdateTask__When__() {
            var defaultUpdateManager = new DefaultUpdateManager(
                XRoadManagerProvider.RequireXRoadManager(),
                new MembersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SecurityServersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SubSystemsStorageUpdater(DbContextProvider.RequireDbContext()),
                new SubSystemServicesStorageUpdater(DbContextProvider.RequireDbContext()),
                new MemberServicesStorageUpdater(DbContextProvider.RequireDbContext())
            );
            await defaultUpdateManager.RunBatchUpdateTask();
        }
    }
}