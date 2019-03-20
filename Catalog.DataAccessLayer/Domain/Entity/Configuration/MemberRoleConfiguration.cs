using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Domain.Entity.Configuration {
    public class MemberRoleConfiguration : IEntityTypeConfiguration<MemberRole> {
        public void Configure(EntityTypeBuilder<MemberRole> builder) {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => entity.Name).IsUnique();

            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(entity => entity.MemberInfoReferences)
                .WithOne(reference => reference.MemberRole)
                .HasForeignKey(reference => reference.MemberRoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}