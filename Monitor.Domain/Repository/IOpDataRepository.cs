using Monitor.Domain.Entity;

namespace Monitor.Domain.Repository
{
    public interface IOpDataRepository
    {
        void InsertRecords(OpDataRecord[] opDataRecords);
    }
}