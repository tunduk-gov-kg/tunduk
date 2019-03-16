using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.XRoad;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service
{
    public class DefaultUpdateManager : IUpdateManager
    {
        private readonly IXRoadManager _xRoadManager;

        private readonly MembersStorageUpdater _membersStorage;
        private readonly SubSystemsStorageUpdater _subSystemsStorage;
        private readonly MemberServicesStorageUpdater _memberServicesStorage;
        private readonly SecurityServersStorageUpdater _serversStorageUpdater;
        private readonly SubSystemServicesStorageUpdater _subSystemServicesStorage;


        public DefaultUpdateManager(IXRoadManager xRoadManager
            , MembersStorageUpdater membersStorage
            , SecurityServersStorageUpdater serversStorageUpdater
            , SubSystemsStorageUpdater subSystemsStorage
            , SubSystemServicesStorageUpdater subSystemServicesStorage
            , MemberServicesStorageUpdater memberServicesStorage)
        {
            _xRoadManager = xRoadManager;
            _membersStorage = membersStorage;
            _serversStorageUpdater = serversStorageUpdater;
            _subSystemsStorage = subSystemsStorage;
            _subSystemServicesStorage = subSystemServicesStorage;
            _memberServicesStorage = memberServicesStorage;
        }


        public async Task RunBatchUpdateTask()
        {
            var memberDataRecords = await _xRoadManager.GetMembersListAsync();
            await _membersStorage.UpdateLocalDatabaseAsync(memberDataRecords);

            var securityServerDataRecords = await _xRoadManager.GetSecurityServersListAsync();
            await _serversStorageUpdater.UpdateLocalDatabaseAsync(securityServerDataRecords);

            var subSystemIdentifiers = await _xRoadManager.GetSubSystemsListAsync();
            await _subSystemsStorage.UpdateLocalDatabaseAsync(subSystemIdentifiers);

            var servicesList = await _xRoadManager.GetServicesListAsync();

            var containsSubSystemCode =
                new Predicate<ServiceIdentifier>(identifier => string.IsNullOrEmpty(identifier.SubSystemCode));

            await _memberServicesStorage.UpdateLocalDatabaseAsync(
                ImmutableList.CreateRange(
                    servicesList.Where(service => containsSubSystemCode(service))));

            await _subSystemServicesStorage.UpdateLocalDatabaseAsync(
                ImmutableList.CreateRange(
                    servicesList.Where(service => !containsSubSystemCode(service))));
            
            foreach (var serviceIdentifier in servicesList)
            {
                
            }
        }

        public async Task RunWsdlUpdateTask(ServiceIdentifier targetService)
        {
            throw new NotImplementedException();
        }
    }
}