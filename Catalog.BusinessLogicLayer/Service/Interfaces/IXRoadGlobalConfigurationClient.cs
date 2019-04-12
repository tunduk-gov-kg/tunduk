using System.Collections.Generic;
using System.Threading.Tasks;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.Interfaces
{
    public interface IXRoadGlobalConfigurationClient
    {
        Task<IList<MemberData>> GetMembersListAsync();
        Task<IList<SecurityServerData>> GetSecurityServersListAsync();
        Task<IList<SubSystemIdentifier>> GetSubSystemsListAsync();
        Task<IList<ServiceIdentifier>> GetServicesListAsync();
        Task<string> GetWsdlAsync(ServiceIdentifier targetService);
    }
}