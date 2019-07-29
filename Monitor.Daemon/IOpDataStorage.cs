using Monitor.Domain.Entity;

namespace Monitor.Daemon
{
    public interface IOpDataStorage
    {
        void Store(OpDataRecord[] dataRecords);
    }
}