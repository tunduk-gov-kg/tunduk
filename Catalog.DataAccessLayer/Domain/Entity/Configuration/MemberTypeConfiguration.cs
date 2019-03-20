using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Domain.Entity.Configuration {
    public class MemberTypeConfiguration : IEntityTypeConfiguration<MemberType> {
        public void Configure(EntityTypeBuilder<MemberType> builder) {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => entity.Name).IsUnique();

            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(200);

            builder.HasMany(entity => entity.MemberInfoRecords)
                .WithOne(record => record.MemberType)
                .HasForeignKey(record => record.MemberTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}