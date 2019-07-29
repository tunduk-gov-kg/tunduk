using System;
using System.Collections.Generic;
using System.Linq;
using Monitor.Domain;
using Monitor.Domain.Entity;
using XRoad.Domain;

namespace Monitor.Daemon.Service
{
    public class ServerManager : IServerManager, IDisposable
    {
        private readonly MonitorDbContext _dbContext;

        public ServerManager(MonitorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Server> GetServersList()
        {
            return _dbContext.Servers.ToList();
        }

        public void UpdateServersList(IEnumerable<SecurityServerData> serversList)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var localCache = _dbContext.Servers.ToList();
                var forRemove = localCache.Where(server => !serversList.Any(server.SameAs)).ToList();
                _dbContext.Servers.RemoveRange(forRemove);

                var forAdd = serversList.Where(incomingServer => !localCache.Any(incomingServer.SameAs))
                    .ToList().ConvertAll(it => it.AsEntity());

                _dbContext.Servers.AddRange(forAdd);
                _dbContext.SaveChanges();
                transaction.Commit();
            }
        }

        public void UpdateServer(Server server)
        {
            _dbContext.Update(server);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}