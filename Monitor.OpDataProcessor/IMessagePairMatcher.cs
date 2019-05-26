using Monitor.Domain.Entity;

namespace Monitor.OpDataProcessor
{
    public interface IMessagePairMatcher
    {
        Message FindMessage(OpDataRecord dataRecord);
    }
}