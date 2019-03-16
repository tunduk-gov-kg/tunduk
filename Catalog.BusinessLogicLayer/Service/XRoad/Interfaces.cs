using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.XRoad
{
    public interface IXRoadStorageUpdater<T> : IDisposable
    {
        Task UpdateLocalDatabaseAsync(IImmutableList<T> updatedList);
    }

    public interface IXRoadManager
    {
        Task<ImmutableList<MemberData>> GetMembersListAsync();
        Task<ImmutableList<SecurityServerData>> GetSecurityServersListAsync();
        Task<ImmutableList<SubSystemIdentifier>> GetSubSystemsListAsync();
        Task<ImmutableList<ServiceIdentifier>> GetServicesListAsync();
        Task<string> GetWsdlAsync(ServiceIdentifier targetService);
    }
}