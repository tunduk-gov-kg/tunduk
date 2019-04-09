using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration
{
    public class CatalogUserConfiguration : IEntityTypeConfiguration<CatalogUser>
    {
        public void Configure(EntityTypeBuilder<CatalogUser> builder)
        {
            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(100);
        }
    }
}