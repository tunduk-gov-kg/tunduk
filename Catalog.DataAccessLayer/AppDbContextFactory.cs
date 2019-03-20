using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Catalog.DataAccessLayer {
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> {
        public AppDbContext CreateDbContext(string[] args) {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=Tunduk;User Id=postgres;Password=postgres;")
                .Options;

            return new AppDbContext(options);
        }
    }
}