using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests {
    public class UpdateManagerUnitTests {
        [Fact]
        public async Task RunBatchUpdateTask__When__() {
            var defaultUpdateManager = new UpdaterManager(
                XRoadManagerProvider.RequireXRoadManager(),
                new MembersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SecurityServersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SubSystemsStorageUpdater(DbContextProvider.RequireDbContext()),
                new ServicesStorageUpdater(DbContextProvider.RequireDbContext()));
            await defaultUpdateManager.RunBatchUpdateTask();
        }
    }
}