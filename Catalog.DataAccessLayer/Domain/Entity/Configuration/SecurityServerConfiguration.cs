using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Domain.Entity.Configuration {
    internal class SecurityServerConfiguration : IEntityTypeConfiguration<SecurityServer> {
        public void Configure(EntityTypeBuilder<SecurityServer> builder) {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => new {
                entity.MemberId,
                entity.SecurityServerCode
            }).IsUnique();

            builder.Property(entity => entity.MemberId).IsRequired();
            builder.Property(entity => entity.SecurityServerCode).IsRequired().HasMaxLength(100);
            builder.Property(entity => entity.Address).IsRequired().HasMaxLength(500);
            builder.Property(entity => entity.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.Property(entity => entity.CreatedAt).IsRequired()
                .HasDefaultValueSql("now()");

            builder.Property(entity => entity.ModifiedAt).IsRequired(false)
                .ValueGeneratedOnUpdate();

            builder.HasOne(server => server.Member)
                .WithMany(member => member.SecurityServers)
                .IsRequired()
                .HasForeignKey(entity => entity.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}