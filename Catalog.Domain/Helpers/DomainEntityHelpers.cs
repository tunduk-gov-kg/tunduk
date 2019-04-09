using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Domain.Helpers
{
    public static class DomainEntityHelpers
    {
        public static void ConfigureSoftDeleteProperties<T>(this EntityTypeBuilder<T> type)
            where T : class, ISoftDelete
        {
            type.Property(entity => entity.IsDeleted).IsRequired();
            type.HasQueryFilter(entity => !entity.IsDeleted);
        }

        public static void ConfigureBaseEntityProperties<T>(this EntityTypeBuilder<T> type)
            where T : BaseEntity
        {
            type.HasKey(entity => entity.Id);
            type.Property(entity => entity.CreatedAt).IsRequired();
            type.Property(entity => entity.ModifiedAt).IsRequired(false);
        }

        public static void ConfigureUserTrackableEntity<T>(this EntityTypeBuilder<T> type)
            where T : UserTrackableEntity
        {
            type.ConfigureBaseEntityProperties();
            type.Property(entity => entity.CreatedBy).IsRequired(false).HasMaxLength(500);
            type.Property(entity => entity.ModifiedBy).IsRequired(false).HasMaxLength(500);
            type.HasIndex(entity => entity.CreatedBy);
            type.HasIndex(entity => entity.ModifiedBy);
        }
    }
}