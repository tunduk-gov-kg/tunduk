using System.Threading.Tasks;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.Interfaces {
    public interface IUpdateManager {
        Task RunBatchUpdateTask();
        Task RunWsdlUpdateTask(ServiceIdentifier targetService);
    }
}