using System.Collections.Immutable;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.Service.XRoad;
using XRoad.Domain;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests {
    public class MemberServiceStorageUpdateUnitTests {
        [Fact]
        public async Task UpdateLocalDatabaseAsync_When__() {
            var dbContext = DbContextProvider.RequireDbContext();
            var storageUpdater = new MemberServicesStorageUpdater(dbContext);
            await storageUpdater.UpdateLocalDatabaseAsync(
                ImmutableList.CreateRange(new[] {
                    new ServiceIdentifier {
                        Instance = "KG",
                        MemberClass = "GOV",
                        MemberCode = "1000",
                        ServiceCode = "MemberService"
                    }
                }));
        }
    }
}