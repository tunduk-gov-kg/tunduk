using System;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.Interfaces
{
    public interface IMonitoringService
    {
        void RunOpDataCollectorTask();

        DateTime RunOpDataCollectorTask(SecurityServerIdentifier securityServerIdentifier, DateTime from);
    }
}