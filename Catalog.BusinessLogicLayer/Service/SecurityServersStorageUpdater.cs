using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.DataAccessLayer;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service {
    public class SecurityServersStorageUpdater : IXRoadStorageUpdater<SecurityServerData> {
        private readonly CatalogDbContext _dbContext;

        public SecurityServersStorageUpdater(CatalogDbContext catalogDbContext) {
            _dbContext = catalogDbContext;
        }

        public void Dispose() {
            _dbContext.Dispose();
        }

        public async Task UpdateLocalDatabaseAsync(IImmutableList<SecurityServerData> incomingServersList) {
            var databaseServersList = _dbContext.SecurityServers
                .IgnoreQueryFilters()
                .Include(server => server.Member)
                .ToImmutableList();

            RestorePreviouslyRemovedServers(incomingServersList, databaseServersList);
            RemoveNonExistingServers(incomingServersList, databaseServersList);
            CreateCompletelyNewServers(incomingServersList, databaseServersList);
            UpdateSecurityServersAddresses(incomingServersList, databaseServersList);

            await _dbContext.SaveChangesAsync();
        }

        private void CreateCompletelyNewServers(IImmutableList<SecurityServerData> incomingServersList,
            ImmutableList<SecurityServer> databaseServersList) {
            foreach (var incomingServerData in incomingServersList) {
                var storedInDatabase =
                    databaseServersList.Any(server => Equals(server, incomingServerData.SecurityServerIdentifier));

                if (storedInDatabase) continue;

                var findContextMember = new Predicate<Member>(member =>
                    member.Instance.Equals(incomingServerData.SecurityServerIdentifier.Instance)
                    && member.MemberClass.Equals(incomingServerData.SecurityServerIdentifier.MemberClass)
                    && member.MemberCode.Equals(incomingServerData.SecurityServerIdentifier.MemberCode));

                var contextMember = _dbContext.Members.FirstOrDefault(member => findContextMember(member));

                if (contextMember == null) continue;

                var completelyNewSecurityServer = new SecurityServer {
                    Member = contextMember,
                    Address = incomingServerData.Address,
                    SecurityServerCode = incomingServerData.SecurityServerIdentifier.SecurityServerCode
                };

                _dbContext.SecurityServers.Add(completelyNewSecurityServer);
            }
        }

        private void RemoveNonExistingServers(IImmutableList<SecurityServerData> incomingServersList,
            ImmutableList<SecurityServer> databaseServersList) {
            foreach (var storedInDatabaseServer in databaseServersList) {
                var storedInNewList = incomingServersList.Any(server =>
                    Equals(storedInDatabaseServer, server.SecurityServerIdentifier));
                if (!storedInNewList) _dbContext.SecurityServers.Remove(storedInDatabaseServer);
            }
        }

        private void RestorePreviouslyRemovedServers(IImmutableList<SecurityServerData> incomingServersList,
            ImmutableList<SecurityServer> databaseServersList) {
            foreach (var storedInDatabaseServer in databaseServersList) {
                var storedInNewList = incomingServersList.Any(server =>
                    Equals(storedInDatabaseServer, server.SecurityServerIdentifier));
                if (!storedInNewList || !storedInDatabaseServer.IsDeleted) continue;
                storedInDatabaseServer.IsDeleted = false;
                _dbContext.SecurityServers.Update(storedInDatabaseServer);
            }
        }

        private void UpdateSecurityServersAddresses(IImmutableList<SecurityServerData> incomingServersList,
            ImmutableList<SecurityServer> databaseServersList) {
            foreach (var storedInDatabaseServer in databaseServersList) {
                var newServerData = incomingServersList.FirstOrDefault(server =>
                    Equals(storedInDatabaseServer, server.SecurityServerIdentifier));
                if (newServerData == null) continue;
                storedInDatabaseServer.Address = newServerData.Address;
                _dbContext.SecurityServers.Update(storedInDatabaseServer);
            }
        }

        private bool Equals(SecurityServer server, SecurityServerIdentifier serverIdentifier) {
            return server.SecurityServerCode.Equals(serverIdentifier.SecurityServerCode)
                && server.Member.Instance.Equals(serverIdentifier.Instance)
                && server.Member.MemberClass.Equals(serverIdentifier.MemberClass)
                && server.Member.MemberCode.Equals(serverIdentifier.MemberCode);
        }
    }
}