using Ardalis.SmartEnum;

namespace Catalog.Domain.Enum
{
    public sealed class MessageState : SmartEnum<MessageState>
    {
        public static readonly MessageState MergedConsumer = new MessageState("MergedConsumer", 1);
        public static readonly MessageState MergedProducer = new MessageState("MergedProducer", 2);
        public static readonly MessageState MergedAll = new MessageState("MergedAll", 3);
        private MessageState(string name, int value) : base(name, value) { }
    }
}