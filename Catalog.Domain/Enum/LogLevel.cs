using Ardalis.SmartEnum;

namespace Catalog.Domain.Enum {
    public sealed class LogLevel : SmartEnum<LogLevel> {
        public static LogLevel Error = new LogLevel("Error", 1);
        public static LogLevel Information = new LogLevel("Information", 2);

        private LogLevel(string name, int value) : base(name, value) {
        }
    }
}