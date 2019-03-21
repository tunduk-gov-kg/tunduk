using System;

namespace Catalog.DataAccessLayer.Domain.Entity.Interfaces {
    public interface IDateTimeTrackable {
        DateTime CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}