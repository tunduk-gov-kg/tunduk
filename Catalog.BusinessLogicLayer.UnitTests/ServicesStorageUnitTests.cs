using System.Collections.Immutable;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using XRoad.Domain;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests {
    public class ServicesStorageUnitTests {
        [Fact]
        public async Task UpdateLocalDatabaseAsync_When__() {
            var dbContext      = DbContextProvider.RequireDbContext();
            var storageUpdater = new ServicesStorageUpdater(dbContext);
            await storageUpdater.UpdateLocalDatabaseAsync(ImmutableList.CreateRange(
                new[] {
                    new ServiceIdentifier {
                        Instance = "KG",
                        MemberClass = "GOV",
                        MemberCode = "1000",
                        SubSystemCode = "MICROSOFT_OFFICE",
                        ServiceCode = "RegisterPerson",
                        ServiceVersion = "v2"
                    }
                }));
        }
    }
}