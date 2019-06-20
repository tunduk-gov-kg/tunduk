using System.Collections.Generic;
using System.Linq;
using Monitor.Domain;
using Monitor.Domain.Entity;
using XRoad.Domain;

namespace Monitor.OpDataCollector.Extensions
{
    public static class SecurityServerExtensions
    {
        public static SecurityServerIdentifier GetIdentifier(this Server server)
        {
            return new SecurityServerIdentifier
            {
                Instance = server.Instance,
                MemberClass = server.MemberClass,
                MemberCode = server.MemberCode,
                SecurityServerCode = server.Code
            };
        }

        public static void UpdateServersList(this MonitorDbContext dbContext,
            IEnumerable<SecurityServerData> serversList)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var localCache = dbContext.Servers.ToList();
                var forRemove = localCache.Where(server => !serversList.Any(server.SameAs)).ToList();
                dbContext.Servers.RemoveRange(forRemove);

                var forAdd = serversList.Where(incomingServer => !localCache.Any(incomingServer.SameAs))
                    .ToList().ConvertAll(it => it.AsEntity());

                dbContext.Servers.AddRange(forAdd);
                dbContext.SaveChanges();
                transaction.Commit();
            }
        }
    }
}