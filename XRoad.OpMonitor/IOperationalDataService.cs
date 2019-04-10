using System.Threading.Tasks;
using XRoad.Domain;
using XRoad.OpMonitor.Domain;
using XRoad.OpMonitor.Domain.SOAP;

namespace XRoad.OpMonitor
{
    public interface IOperationalDataService
    {
        OperationalData GetOperationalData(
            XRoadExchangeParameters xRoadExchangeParameters,
            SecurityServerIdentifier securityServerIdentifier,
            SearchCriteria searchCriteria
        );
    }
}