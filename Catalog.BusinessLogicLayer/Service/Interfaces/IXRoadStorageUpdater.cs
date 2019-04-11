using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.BusinessLogicLayer.Service.Interfaces
{
    public interface IXRoadStorageUpdater<T> : IDisposable
    {
        Task UpdateLocalDatabaseAsync(IList<T> updatedList);
    }
}