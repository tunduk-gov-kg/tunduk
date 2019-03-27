using Catalog.Domain.Entity;
using Catalog.Domain.Enum;
using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration {
    public class DomainLogConfiguration : IEntityTypeConfiguration<DomainLog> {
        public void Configure(EntityTypeBuilder<DomainLog> builder) {
            builder.ConfigureBaseEntityProperties();
            builder.HasIndex(entity => entity.LogLevel);

            builder.Property(entity => entity.LogLevel)
                .HasConversion(entity => entity.Name,
                    dbValue => LogLevel.FromName(dbValue,false))
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(entity => entity.Message)
                .HasColumnType("text");
            
            builder.Property(entity => entity.Description)
                .HasColumnType("text");
        }
    }
}