using Catalog.DataAccessLayer.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.DataAccessLayer
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=Tunduk;User Id=postgres;Password=postgres;")
                .Options;

            return new CatalogDbContext(options, new MockUserIdProvider());
        }
    }
}