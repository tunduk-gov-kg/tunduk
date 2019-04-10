using Catalog.DataAccessLayer;

namespace Catalog.BusinessLogicLayer.UnitTests.Providers
{
    internal static class DbContextProvider
    {
        public static CatalogDbContext RequireDbContext()
        {
            var dbContextFactory = new CatalogDbContextFactory();
            return dbContextFactory.CreateDbContext(new string[] { });
        }
    }
}