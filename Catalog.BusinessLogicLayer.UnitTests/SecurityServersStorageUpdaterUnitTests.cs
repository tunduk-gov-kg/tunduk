using System;
using System.Collections.Immutable;
using System.Linq;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.UnitTests.Providers;
using XRoad.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class SecurityServersStorageUpdaterUnitTests
    {
        private ITestOutputHelper _helper;

        public SecurityServersStorageUpdaterUnitTests(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public async void UpdateLocalDatabaseAsync_When__()
        {
            var appDbContext = DbContextProvider.RequireDbContext();

            var storage = new SecurityServersStorageUpdater(appDbContext);
            using (storage)
            {
                await storage.UpdateLocalDatabaseAsync(ImmutableList.CreateRange(
                    new[]
                    {
                        new SecurityServerData
                        {
                            Address = "http://tunduk.com",
                            SecurityServerIdentifier = new SecurityServerIdentifier
                            {
                                Instance = "KG",
                                MemberClass = "GOV",
                                MemberCode = "2000",
                                SecurityServerCode = "GOOGLE-SERVER"
                            }
                        }
                    }));
            }
        }


        [Fact]
        public void GetSecurityServers()
        {
            var requireDbContext = DbContextProvider.RequireDbContext();

            var securityServers = requireDbContext.SecurityServers.ToList();
            foreach (var securityServer in securityServers)
            {
                _helper.WriteLine(securityServer.ToString());
            }
        }
    }
}