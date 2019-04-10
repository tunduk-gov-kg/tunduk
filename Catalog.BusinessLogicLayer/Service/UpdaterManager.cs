using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Microsoft.Extensions.Logging;
using SimpleSOAPClient.Exceptions;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service
{
    public class UpdaterManager : IUpdateManager
    {
        private readonly MembersStorageUpdater _membersStorage;
        private readonly SecurityServersStorageUpdater _serversStorageUpdater;
        private readonly ServicesStorageUpdater _servicesStorage;
        private readonly SubSystemsStorageUpdater _subSystemsStorage;
        private readonly IXRoadManager _xRoadManager;
        private readonly ILogger<UpdaterManager> _logger;

        public UpdaterManager(IXRoadManager xRoadManager
            , MembersStorageUpdater membersStorage
            , SecurityServersStorageUpdater serversStorageUpdater
            , SubSystemsStorageUpdater subSystemsStorage
            , ServicesStorageUpdater servicesStorage
            , ILogger<UpdaterManager> logger)
        {
            _xRoadManager = xRoadManager;
            _membersStorage = membersStorage;
            _serversStorageUpdater = serversStorageUpdater;
            _subSystemsStorage = subSystemsStorage;
            _servicesStorage = servicesStorage;
            _logger = logger;
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
                new Predicate<ServiceIdentifier>(identifier => !string.IsNullOrEmpty(identifier.SubSystemCode));

            var subSystemServicesList = servicesList.Where(service => containsSubSystemCode(service)).ToImmutableList();
            await _servicesStorage.UpdateLocalDatabaseAsync(subSystemServicesList);

            await UpdateServicesWsdl(servicesList);
        }

        public async Task RunWsdlUpdateTask(ServiceIdentifier targetService)
        {
            try
            {
                var wsdl = await _xRoadManager.GetWsdlAsync(targetService);
                await _servicesStorage.UpdateWsdlAsync(targetService, wsdl);
            }
            catch (FaultException exception)
            {
                _logger.LogError(LoggingEvents.UpdateWsdlTask, "" +
                    "Error occurred during wsdl update task for service: {service}; " +
                    "Server responded with message: {message}",
                    targetService.ToString(),
                    exception.String
                );
            }
        }

        private async Task UpdateServicesWsdl(ImmutableList<ServiceIdentifier> subSystemServicesList)
        {
            foreach (var serviceIdentifier in subSystemServicesList.AsParallel())
                await RunWsdlUpdateTask(serviceIdentifier);
        }
    }
}