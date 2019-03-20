using System.Collections.Immutable;
using Catalog.BusinessLogicLayer.Service.XRoad;
using Catalog.DataAccessLayer;
using XRoad.Domain;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests {
    public class SecurityServersStorageUpdaterUnitTests {
        [Fact]
        public async void UpdateLocalDatabaseAsync_When__() {
            var appDbContext = new AppDbContextFactory().CreateDbContext(new string[] { });

            var storage = new SecurityServersStorageUpdater(appDbContext);
            using (storage) {
                await storage.UpdateLocalDatabaseAsync(ImmutableList.CreateRange(
                    new[] {
                        new SecurityServerData {
                            Address = "http://tunduk.com",
                            SecurityServerIdentifier = new SecurityServerIdentifier {
                                Instance = "KG",
                                MemberClass = "GOV",
                                MemberCode = "2000",
                                SecurityServerCode = "GOOGLE-SERVER"
                            }
                        }
                    }));
            }
        }
    }
}