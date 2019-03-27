using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.Domain.Enum;
using SimpleSOAPClient.Exceptions;
using XRoad.Domain;
using XRoad.GlobalConfiguration;
using XRoad.GlobalConfiguration.Metadata;

namespace Catalog.BusinessLogicLayer.Service {
    public class XRoadManager : IXRoadManager {
        private readonly IServiceMetadataManager _serviceMetadataManager;
        private readonly XRoadExchangeParameters _xRoadExchangeParameters;
        private readonly IDomainLogger _logger;

        public XRoadManager(IServiceMetadataManager serviceMetadataManager
            , XRoadExchangeParameters xRoadExchangeParameters
            , IDomainLogger logger
        ) {
            _serviceMetadataManager = serviceMetadataManager;
            _xRoadExchangeParameters = xRoadExchangeParameters;
            _logger = logger;
        }

        public async Task<ImmutableList<MemberData>> GetMembersListAsync() {
            var sharedParams =
                await _serviceMetadataManager.GetSharedParamsAsync(_xRoadExchangeParameters.SecurityServerUri);

            var converter = new Converter<Member, MemberData>(member => new MemberData {
                Name = member.Name,
                MemberIdentifier = new MemberIdentifier {
                    Instance = sharedParams.InstanceIdentifier,
                    MemberClass = member.MemberClass.Code,
                    MemberCode = member.MemberCode
                }
            });

            return ImmutableList.CreateRange(
                sharedParams.Members.ConvertAll(converter)
            );
        }

        public async Task<ImmutableList<SecurityServerData>> GetSecurityServersListAsync() {
            var sharedParams =
                await _serviceMetadataManager.GetSharedParamsAsync(_xRoadExchangeParameters.SecurityServerUri);

            var converter = new Converter<SecurityServer, SecurityServerData>(input => {
                var contextMember = FindMember(sharedParams, input.Owner);
                return new SecurityServerData {
                    SecurityServerIdentifier = new SecurityServerIdentifier {
                        Instance = sharedParams.InstanceIdentifier,
                        MemberClass = contextMember.MemberClass.Code,
                        MemberCode = contextMember.MemberCode,
                        SecurityServerCode = input.ServerCode
                    },
                    Address = input.Address
                };
            });

            return ImmutableList.CreateRange(
                sharedParams.SecurityServers.ConvertAll(converter)
            );
        }

        public async Task<ImmutableList<SubSystemIdentifier>> GetSubSystemsListAsync() {
            var sharedParams =
                await _serviceMetadataManager.GetSharedParamsAsync(_xRoadExchangeParameters.SecurityServerUri);
            var converter = new Converter<Member, List<SubSystemIdentifier>>(member => {
                return member.SubSystems.ConvertAll(subSystem => new SubSystemIdentifier {
                    Instance = sharedParams.InstanceIdentifier,
                    MemberClass = member.MemberClass.Code,
                    MemberCode = member.MemberCode,
                    SubSystemCode = subSystem.SubSystemCode
                });
            });
            return ImmutableList.CreateRange(sharedParams.Members.ConvertAll(converter).SelectMany(list => list));
        }

        public async Task<ImmutableList<ServiceIdentifier>> GetServicesListAsync() {
            var subSystemIdentifiers = await GetSubSystemsListAsync();
            var servicesList         = new List<ServiceIdentifier>();

            foreach (var subSystemIdentifier in subSystemIdentifiers)
                try {
                    var serviceIdentifiers = await _serviceMetadataManager.GetServicesAsync(
                        _xRoadExchangeParameters.SecurityServerUri,
                        _xRoadExchangeParameters.ClientSubSystem, subSystemIdentifier);

                    servicesList.AddRange(serviceIdentifiers);
                }
                catch (FaultException exception) {
                    _logger.Log(LogLevel.Error, exception.Code, exception.String);
                }

            return ImmutableList.CreateRange(servicesList);
        }

        public async Task<string> GetWsdlAsync(ServiceIdentifier targetService) {
            var wsdlBytes = await _serviceMetadataManager.GetWsdlAsync(_xRoadExchangeParameters.SecurityServerUri,
                _xRoadExchangeParameters.ClientSubSystem, targetService);
            return Encoding.UTF8.GetString(wsdlBytes);
        }

        private Member FindMember(SharedParams sharedParams, string id) {
            return sharedParams.Members.First(member => member.Id.Equals(id));
        }
    }
}