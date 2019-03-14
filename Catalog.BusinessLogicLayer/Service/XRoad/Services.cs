using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Catalog.DataAccessLayer.XRoad.Entity;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.XRoad
{
    public interface IXRoadStorageUpdater<T> : IDisposable
    {
        Task UpdateLocalDatabaseAsync(IImmutableList<T> updatedList);
    }
}
