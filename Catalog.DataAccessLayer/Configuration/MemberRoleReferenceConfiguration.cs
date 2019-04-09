using Catalog.Domain.Entity;
using Catalog.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration
{
    public class MemberRoleReferenceConfiguration : IEntityTypeConfiguration<MemberRoleReference>
    {
        public void Configure(EntityTypeBuilder<MemberRoleReference> builder)
        {
            builder.HasKey(entity => new
            {
                entity.MemberRole,
                entity.MemberId
            });

            builder.HasOne(entity => entity.Member)
                .WithMany(member => member.MemberRoles)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(entity => entity.MemberId);

            builder.Property(entity => entity.MemberRole)
                .HasMaxLength(200)
                .HasConversion(entity => entity.Name,
                    value => MemberRole.FromName(value, false));
        }
    }
}