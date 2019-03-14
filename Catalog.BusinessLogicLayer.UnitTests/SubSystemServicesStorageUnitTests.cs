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
    public class SubSystemServicesStorageUnitTests
    {
        [Fact]
        public async Task UpdateLocalDatabaseAsync_When__()
        {
            var dbContext = DbContextProvider.RequireDbContext();
            var storageUpdater = new SubSystemServicesStorageUpdater(dbContext);
            await storageUpdater.UpdateLocalDatabaseAsync(ImmutableList.CreateRange<ServiceIdentifier>(
                new ServiceIdentifier[]
                {
                    new ServiceIdentifier()
                    {
                        Instance = "KG",
                        MemberClass ="GOV",
                        MemberCode = "1000",
                        SubSystemCode = "MICROSOFT_OFFICE",
                        ServiceCode = "RegisterPerson",
                        ServiceVersion = "v2"
                    }
                }));
        }
    }
}
