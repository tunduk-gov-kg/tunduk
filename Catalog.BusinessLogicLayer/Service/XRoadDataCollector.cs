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
    public class XRoadDataCollector
    {
        private readonly IXRoadStorageUpdater<MemberData> _membersStorage;
        private readonly IXRoadStorageUpdater<SecurityServerData> _serversStorageUpdater;
        private readonly IXRoadStorageUpdater<SubSystemIdentifier> _subSystemsStorage;
        private readonly ServicesStorageUpdater _servicesStorage;
        private readonly IXRoadGlobalConfigurationClient _configurationClient;
        private readonly ILogger<XRoadDataCollector> _logger;

        public XRoadDataCollector(IXRoadGlobalConfigurationClient configurationClient
            , IXRoadStorageUpdater<MemberData> membersStorage
            , SecurityServersStorageUpdater serversStorageUpdater
            , SubSystemsStorageUpdater subSystemsStorage
            , ServicesStorageUpdater servicesStorage
            , ILogger<XRoadDataCollector> logger)
        {
            _configurationClient = configurationClient;
            _membersStorage = membersStorage;
            _serversStorageUpdater = serversStorageUpdater;
            _subSystemsStorage = subSystemsStorage;
            _servicesStorage = servicesStorage;
            _logger = logger;
        }


        public async Task RunBatchUpdateTask()
        {
            var memberDataRecords = await _configurationClient.GetMembersListAsync();
            await _membersStorage.UpdateLocalDatabaseAsync(memberDataRecords);

            var securityServerDataRecords = await _configurationClient.GetSecurityServersListAsync();
            await _serversStorageUpdater.UpdateLocalDatabaseAsync(securityServerDataRecords);

            var subSystemIdentifiers = await _configurationClient.GetSubSystemsListAsync();
            await _subSystemsStorage.UpdateLocalDatabaseAsync(subSystemIdentifiers);

            var servicesList = await _configurationClient.GetServicesListAsync();

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
                var wsdl = await _configurationClient.GetWsdlAsync(targetService);
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