using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Catalog.Entity.Configuration
{
    public class MemberInfoRoleReferenceConfiguration : IEntityTypeConfiguration<MemberInfoRoleReference>
    {
        public void Configure(EntityTypeBuilder<MemberInfoRoleReference> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => new
            {
                entity.MemberInfoId,
                entity.MemberRoleId
            }).IsUnique();

            builder.HasOne(entity => entity.MemberInfo)
                .WithMany(memberInfo => memberInfo.MemberRoleReferences)
                .IsRequired()
                .HasForeignKey(entity => entity.MemberInfoId);

            builder.HasOne(entity => entity.MemberRole)
                .WithMany(memberRole => memberRole.MemberInfoReferences)
                .IsRequired()
                .HasForeignKey(entity => entity.MemberRoleId);
        }
    }
}