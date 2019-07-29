using System.Collections.Generic;
using Monitor.Domain.Entity;
using XRoad.Domain;

namespace Monitor.Daemon
{
    public interface IServerManager
    {
        IEnumerable<Server> GetServersList();
        void UpdateServersList(IEnumerable<SecurityServerData> serversList);
        void UpdateServer(Server server);
    }
}