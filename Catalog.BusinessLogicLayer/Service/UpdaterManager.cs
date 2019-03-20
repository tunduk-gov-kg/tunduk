using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.BusinessLogicLayer.Service.XRoad;
using NLog;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service {
    public class UpdaterManager : IUpdateManager {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MemberServicesStorageUpdater _memberServicesStorage;

        private readonly MembersStorageUpdater _membersStorage;
        private readonly SecurityServersStorageUpdater _serversStorageUpdater;
        private readonly SubSystemServicesStorageUpdater _subSystemServicesStorage;
        private readonly SubSystemsStorageUpdater _subSystemsStorage;
        private readonly IXRoadManager _xRoadManager;

        public UpdaterManager(IXRoadManager xRoadManager
            , MembersStorageUpdater membersStorage
            , SecurityServersStorageUpdater serversStorageUpdater
            , SubSystemsStorageUpdater subSystemsStorage
            , SubSystemServicesStorageUpdater subSystemServicesStorage
            , MemberServicesStorageUpdater memberServicesStorage) {
            _xRoadManager = xRoadManager;
            _membersStorage = membersStorage;
            _serversStorageUpdater = serversStorageUpdater;
            _subSystemsStorage = subSystemsStorage;
            _subSystemServicesStorage = subSystemServicesStorage;
            _memberServicesStorage = memberServicesStorage;
        }


        public async Task RunBatchUpdateTask() {
            var memberDataRecords = await _xRoadManager.GetMembersListAsync();
            await _membersStorage.UpdateLocalDatabaseAsync(memberDataRecords);

            var securityServerDataRecords = await _xRoadManager.GetSecurityServersListAsync();
            await _serversStorageUpdater.UpdateLocalDatabaseAsync(securityServerDataRecords);

            var subSystemIdentifiers = await _xRoadManager.GetSubSystemsListAsync();
            await _subSystemsStorage.UpdateLocalDatabaseAsync(subSystemIdentifiers);

            var servicesList = await _xRoadManager.GetServicesListAsync();

            var containsSubSystemCode =
                new Predicate<ServiceIdentifier>(identifier => !string.IsNullOrEmpty(identifier.SubSystemCode));

            var subSystemServicesList = servicesList.Where(service => containsSubSystemCode(service)).ToImmutableList();
            await _subSystemServicesStorage.UpdateLocalDatabaseAsync(subSystemServicesList);

            var memberServicesList = servicesList.Where(service => !containsSubSystemCode(service)).ToImmutableList();
            await _memberServicesStorage.UpdateLocalDatabaseAsync(memberServicesList);

            await UpdateServicesWsdl(servicesList);
        }

        public async Task RunWsdlUpdateTask(ServiceIdentifier targetService) {
            var wsdl = await _xRoadManager.GetWsdlAsync(targetService);
            if (string.IsNullOrEmpty(targetService.SubSystemCode))
                await _membersStorage.UpdateWsdlAsync(targetService, wsdl);
            else
                await _subSystemServicesStorage.UpdateWsdlAsync(targetService, wsdl);
        }

        private async Task UpdateServicesWsdl(ImmutableList<ServiceIdentifier> subSystemServicesList) {
            foreach (var serviceIdentifier in subSystemServicesList)
                try {
                    await RunWsdlUpdateTask(serviceIdentifier);
                }
                catch (Exception exception) {
                    _logger.Error(exception);
                }
        }
    }
}