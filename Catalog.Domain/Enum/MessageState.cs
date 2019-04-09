using Ardalis.SmartEnum;

namespace Catalog.Domain.Enum
{
    public sealed class MessageState : SmartEnum<MessageState>
    {
        public static MessageState MergedConsumer { get; set; }
        public static MessageState MergedProducer { get; set; }
        public static MessageState MergedAll { get; set; }
        private MessageState(string name, int value) : base(name, value) { }
    }
}