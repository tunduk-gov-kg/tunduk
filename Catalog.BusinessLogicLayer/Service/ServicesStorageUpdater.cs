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
    public class ServicesStorageUpdater : IXRoadStorageUpdater<ServiceIdentifier> {
        private readonly CatalogDbContext _catalogDbContext;

        public ServicesStorageUpdater(CatalogDbContext catalogDbContext) {
            _catalogDbContext = catalogDbContext;
        }

        public async Task UpdateLocalDatabaseAsync(IImmutableList<ServiceIdentifier> updatedServicesList) {
            var databaseServicesList = _catalogDbContext.Services
                .Include(service => service.SubSystem)
                .ThenInclude(subSystem => subSystem.Member)
                .ToImmutableList();

            CreateCompletelyNewService(databaseServicesList, updatedServicesList);
            RemoveNonExistingServices(databaseServicesList, updatedServicesList);
            RestorePreviouslyDeletedServices(databaseServicesList, updatedServicesList);

            await _catalogDbContext.SaveChangesAsync();
        }

        public void Dispose() {
            _catalogDbContext.Dispose();
        }

        public async Task UpdateWsdlAsync(ServiceIdentifier serviceIdentifier, string wsdl) {
            var targetService = _catalogDbContext.Services
                .Include(service => service.SubSystem)
                .ThenInclude(system => system.Member)
                .First(service =>
                    service.ServiceCode == serviceIdentifier.ServiceCode
                    && service.ServiceVersion == serviceIdentifier.ServiceVersion
                    && service.SubSystem.SubSystemCode == serviceIdentifier.SubSystemCode
                    && service.SubSystem.Member.MemberCode == serviceIdentifier.MemberCode
                    && service.SubSystem.Member.MemberClass == serviceIdentifier.MemberClass
                    && service.SubSystem.Member.Instance == serviceIdentifier.Instance);
            targetService.Wsdl = wsdl;
            _catalogDbContext.Services.Update(targetService);
            await _catalogDbContext.SaveChangesAsync();
        }

        private void RestorePreviouslyDeletedServices(ImmutableList<Domain.Entity.Service> databaseServicesList,
            IImmutableList<ServiceIdentifier> updatedServicesList) {
            foreach (var databaseService in databaseServicesList) {
                if (!databaseService.IsDeleted) continue;
                var storedInUpdatedList = updatedServicesList.Any(service => Equals(databaseService, service));
                if (!storedInUpdatedList) continue;
                _catalogDbContext.Services.Update(databaseService);
            }
        }

        private void RemoveNonExistingServices(ImmutableList<Domain.Entity.Service> databaseServicesList,
            IImmutableList<ServiceIdentifier> updatedServicesList) {
            foreach (var databaseService in databaseServicesList) {
                var storedInUpdatedList = updatedServicesList.Any(service => Equals(databaseService, service));
                if (storedInUpdatedList) continue;
                _catalogDbContext.Services.Update(databaseService);
            }
        }

        private void CreateCompletelyNewService(ImmutableList<Domain.Entity.Service> databaseServicesList,
            IImmutableList<ServiceIdentifier> updatedServicesList) {
            foreach (var incomingService in updatedServicesList) {
                var storedInDatabase =
                    databaseServicesList.Any(databaseService => Equals(databaseService, incomingService));
                if (storedInDatabase) continue;

                var isContextSubSystem = new Predicate<SubSystem>(subSystem =>
                    subSystem.SubSystemCode.Equals(incomingService.SubSystemCode)
                    && subSystem.Member.Instance.Equals(incomingService.Instance)
                    && subSystem.Member.MemberClass.Equals(incomingService.MemberClass)
                    && subSystem.Member.MemberCode.Equals(incomingService.MemberCode));

                var contextSubSystem = _catalogDbContext.SubSystems
                    .Include(system => system.Member)
                    .ToList()
                    .FirstOrDefault(subSystem => isContextSubSystem(subSystem));

                if (contextSubSystem == null) continue;

                var completelyNewService = new Domain.Entity.Service {
                    ServiceCode = incomingService.ServiceCode,
                    ServiceVersion = incomingService.ServiceVersion,
                    SubSystem = contextSubSystem
                };
                _catalogDbContext.Services.Add(completelyNewService);
            }
        }

        private bool Equals(Domain.Entity.Service service, ServiceIdentifier serviceIdentifier) {
            var expression = service.ServiceCode.Equals(serviceIdentifier.ServiceCode)
                && service.SubSystem.SubSystemCode.Equals(serviceIdentifier.SubSystemCode)
                && service.SubSystem.Member.Instance.Equals(serviceIdentifier.Instance)
                && service.SubSystem.Member.MemberClass.Equals(serviceIdentifier.MemberClass)
                && service.SubSystem.Member.MemberCode.Equals(serviceIdentifier.MemberCode);

            if (service.ServiceVersion == null) return expression && serviceIdentifier.ServiceVersion == null;

            return expression && service.ServiceVersion.Equals(serviceIdentifier.ServiceVersion);
        }
    }
}