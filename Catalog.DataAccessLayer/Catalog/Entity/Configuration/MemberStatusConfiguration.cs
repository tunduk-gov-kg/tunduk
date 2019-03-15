using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Catalog.Entity.Configuration
{
    public class MemberStatusConfiguration : IEntityTypeConfiguration<MemberStatus>
    {
        public void Configure(EntityTypeBuilder<MemberStatus> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => entity.Name).IsUnique();

            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(200);

            builder.HasMany(entity => entity.MemberInfoRecords)
                .WithOne(record => record.MemberStatus)
                .HasForeignKey(record => record.MemberStatusId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}