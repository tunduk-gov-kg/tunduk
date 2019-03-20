using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.DataAccessLayer;
using Catalog.DataAccessLayer.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service {
    public class SubSystemServicesStorageUpdater : IXRoadStorageUpdater<ServiceIdentifier> {
        private readonly AppDbContext _appDbContext;

        public SubSystemServicesStorageUpdater(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        public async Task UpdateLocalDatabaseAsync(IImmutableList<ServiceIdentifier> updatedServicesList) {
            var databaseServicesList = _appDbContext.SubSystemServices
                .Include(service => service.SubSystem)
                .ThenInclude(subSystem => subSystem.Member)
                .ToImmutableList();

            CreateCompletelyNewService(databaseServicesList, updatedServicesList);
            RemoveNonExistingServices(databaseServicesList, updatedServicesList);
            RestorePreviouslyDeletedServices(databaseServicesList, updatedServicesList);

            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose() {
            _appDbContext.Dispose();
        }

        public async Task UpdateWsdlAsync(ServiceIdentifier serviceIdentifier, string wsdl) {
            var targetService = _appDbContext.SubSystemServices
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
            targetService.ModificationDateTime = DateTime.Now;
            _appDbContext.SubSystemServices.Update(targetService);
            await _appDbContext.SaveChangesAsync();
        }

        private void RestorePreviouslyDeletedServices(ImmutableList<SubSystemService> databaseServicesList,
            IImmutableList<ServiceIdentifier> updatedServicesList) {
            foreach (var databaseService in databaseServicesList) {
                if (!databaseService.IsDeleted) continue;
                var storedInUpdatedList = updatedServicesList.Any(service => Equals(databaseService, service));
                if (!storedInUpdatedList) continue;
                databaseService.IsDeleted = false;
                databaseService.ModificationDateTime = DateTime.Now;
                _appDbContext.SubSystemServices.Update(databaseService);
            }
        }

        private void RemoveNonExistingServices(ImmutableList<SubSystemService> databaseServicesList,
            IImmutableList<ServiceIdentifier> updatedServicesList) {
            foreach (var databaseService in databaseServicesList) {
                var storedInUpdatedList = updatedServicesList.Any(service => Equals(databaseService, service));
                if (storedInUpdatedList) continue;
                databaseService.IsDeleted = true;
                databaseService.ModificationDateTime = DateTime.Now;
                _appDbContext.SubSystemServices.Update(databaseService);
            }
        }

        private void CreateCompletelyNewService(ImmutableList<SubSystemService> databaseServicesList,
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

                var contextSubSystem = _appDbContext.SubSystems
                    .Include(system => system.Member)
                    .ToList()
                    .FirstOrDefault(subSystem => isContextSubSystem(subSystem));

                if (contextSubSystem == null) continue;

                var completelyNewService = new SubSystemService {
                    ServiceCode = incomingService.ServiceCode,
                    ServiceVersion = incomingService.ServiceVersion,
                    SubSystem = contextSubSystem
                };
                _appDbContext.SubSystemServices.Add(completelyNewService);
            }
        }

        private bool Equals(SubSystemService service, ServiceIdentifier serviceIdentifier) {
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