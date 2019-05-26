using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XRoad.Domain;
using XRoad.GlobalConfiguration;
using XRoad.GlobalConfiguration.Metadata;

namespace Monitor.OpDataCollector
{
    public class ServersProvider
    {
        private readonly IServiceMetadataManager _serviceMetadataManager;

        public ServersProvider(IServiceMetadataManager serviceMetadataManager)
        {
            _serviceMetadataManager = serviceMetadataManager;
        }

        public async Task<IList<SecurityServerData>> GetSecurityServersListAsync(Uri securityServerUri)
        {
            var sharedParams =
                await _serviceMetadataManager.GetSharedParamsAsync(securityServerUri);

            var converter = new Converter<SecurityServer, SecurityServerData>(input =>
            {
                var contextMember = FindMember(sharedParams, input.Owner);
                return new SecurityServerData
                {
                    SecurityServerIdentifier = new SecurityServerIdentifier
                    {
                        Instance = sharedParams.InstanceIdentifier,
                        MemberClass = contextMember.MemberClass.Code,
                        MemberCode = contextMember.MemberCode,
                        SecurityServerCode = input.ServerCode
                    },
                    Address = input.Address
                };
            });

            return sharedParams.SecurityServers.ConvertAll(converter);
        }
        
        private Member FindMember(SharedParams sharedParams, string id)
        {
            return sharedParams.Members.First(member => member.Id.Equals(id));
        }
    }
}