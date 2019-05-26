using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Monitor.Domain.Entity;
using XRoad.Domain;

namespace Monitor.Domain.Repository
{
    public class ServerRepository : IServerRepository
    {
        private readonly IDbContextProvider _dbContextProvider;

        public ServerRepository(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public void UpdateLocalCache(IList<SecurityServerData> updatedList)
        {
            using (var monitorDbContext = _dbContextProvider.CreateDbContext())
            using (var transaction = monitorDbContext.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                var localCache = monitorDbContext.Servers.ToList();
                var forRemove = localCache.Where(server => !updatedList.Any(server.SameAs)).ToList();
                monitorDbContext.Servers.RemoveRange(forRemove);

                var forAdd = updatedList.Where(incomingServer => !localCache.Any(incomingServer.SameAs))
                    .ToList().ConvertAll(it => it.AsEntity());

                monitorDbContext.Servers.AddRange(forAdd);

                monitorDbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public void UpdateServer(Server server)
        {
            using (var monitorDbContext = _dbContextProvider.CreateDbContext())
            {
                monitorDbContext.Servers.Update(server);
                monitorDbContext.SaveChanges();
            }
        }

        public IList<Server> GetServersList()
        {
            using (var monitorDbContext = _dbContextProvider.CreateDbContext())
                return monitorDbContext.Servers.ToList();
        }
    }
}