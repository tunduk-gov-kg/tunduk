using System.Collections.Generic;
using Monitor.Domain.Entity;
using XRoad.Domain;

namespace Monitor.Domain.Repository
{
    public interface IServerRepository
    {
        void UpdateLocalCache(IList<SecurityServerData> updatedList);
        void UpdateServer(Server server);
        IList<Server> GetServersList();
    }
}