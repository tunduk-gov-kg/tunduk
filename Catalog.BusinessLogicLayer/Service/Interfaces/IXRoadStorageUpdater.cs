using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Catalog.BusinessLogicLayer.Service.Interfaces
{
    public interface IXRoadStorageUpdater<T> : IDisposable
    {
        Task UpdateLocalDatabaseAsync(IImmutableList<T> updatedList);
    }
}