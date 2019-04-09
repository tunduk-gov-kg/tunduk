using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using XRoad.Domain;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class UpdateManagerUnitTests
    {
        [Fact]
        public async Task RunBatchUpdateTask__When__()
        {
            var defaultUpdateManager = new UpdaterManager(
                XRoadManagerProvider.RequireXRoadManager(),
                new MembersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SecurityServersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SubSystemsStorageUpdater(DbContextProvider.RequireDbContext()),
                new ServicesStorageUpdater(DbContextProvider.RequireDbContext()),
                new DomainLogger(DbContextProvider.RequireDbContext()));
            await defaultUpdateManager.RunBatchUpdateTask();
        }

        [Fact]
        public async Task RunWsdlUpdateTaskAsync__When__()
        {
            var defaultUpdateManager = new UpdaterManager(
                XRoadManagerProvider.RequireXRoadManager(),
                new MembersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SecurityServersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SubSystemsStorageUpdater(DbContextProvider.RequireDbContext()),
                new ServicesStorageUpdater(DbContextProvider.RequireDbContext()),
                new DomainLogger(DbContextProvider.RequireDbContext()));

            await defaultUpdateManager.RunWsdlUpdateTask(
                new ServiceIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000003",
                    SubSystemCode = "settlements-service",
                    ServiceCode = "GetWorkPeriodInfoWithSum"
                });
        }
    }
}