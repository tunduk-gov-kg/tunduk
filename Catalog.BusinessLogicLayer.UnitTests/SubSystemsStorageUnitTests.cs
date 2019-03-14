using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.XRoad;
using XRoad.Domain;
using Xunit;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class SubSystemsStorageUnitTests
    {
        [Fact]
        public async Task UpdateLocalDatabaseAsync_When__()
        {
            var updater = new SubSystemsStorageUpdater(DbContextProvider.RequireDbContext());
            await updater.UpdateLocalDatabaseAsync(ImmutableList.CreateRange(new[]
            {
                new SubSystemIdentifier()
                {
                    Instance = "KG",
                    MemberClass = "GOV",
                    MemberCode = "1000",
                    SubSystemCode = "MICROSOFT_OFFICE"
                }
            }));
        }
    }
}
