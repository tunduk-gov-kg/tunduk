using Catalog.DataAccessLayer;

namespace Catalog.BusinessLogicLayer.UnitTests {
    internal static class DbContextProvider {
        public static CatalogDbContext RequireDbContext() {
            var dbContextFactory = new AppDbContextFactory();
            return dbContextFactory.CreateDbContext(new string[] { });
        }
    }
}