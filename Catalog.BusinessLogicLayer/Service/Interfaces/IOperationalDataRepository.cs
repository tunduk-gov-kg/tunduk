using Catalog.Domain.Entity;

namespace Catalog.BusinessLogicLayer.Service.Interfaces
{
    public interface IOperationalDataRepository
    {
        void InsertRecords(OperationalDataRecord[] operationalDataRecords);
    }
}