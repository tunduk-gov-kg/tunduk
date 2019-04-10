using System.Collections.Immutable;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.UnitTests.Providers;
using XRoad.Domain;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class MembersStorageUpdaterUnitTests
    {
        [Fact]
        public async void UpdateLocalDatabaseAsync_When__()
        {
            var appDbContext = DbContextProvider.RequireDbContext();

            var storage = new MembersStorageUpdater(appDbContext);
            using (storage)
            {
                await storage.UpdateLocalDatabaseAsync(ImmutableList.CreateRange(new[]
                    {
                        new MemberData
                        {
                            MemberIdentifier = new MemberIdentifier
                            {
                                Instance = "KG",
                                MemberClass = "GOV",
                                MemberCode = "2000"
                            },
                            Name = "Sample"
                        }
                    }
                ));
            }
        }
    }
}