using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Catalog.BusinessLogicLayer.Service.XRoad {
    public interface IXRoadStorageUpdater<T> : IDisposable {
        Task UpdateLocalDatabaseAsync(IImmutableList<T> updatedList);
    }
}