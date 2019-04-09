using Catalog.DataAccessLayer;

namespace Catalog.BusinessLogicLayer.UnitTests
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