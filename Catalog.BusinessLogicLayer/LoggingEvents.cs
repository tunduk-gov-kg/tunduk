namespace Catalog.BusinessLogicLayer
{
    public sealed class LoggingEvents
    {
        private LoggingEvents() { }

        public const int GetOperationalData = 1000;
        public const int UpdateWsdlTask = 1001;
        public const int GetServicesList = 1002;
    }
}