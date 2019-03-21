namespace Catalog.DataAccessLayer.Domain.Entity.Interfaces {
    public interface IUserTrackable<TUserId> where TUserId : class {
        TUserId ModifierId { get; set; }
        TUserId CreatorId { get; set; }
    }
}