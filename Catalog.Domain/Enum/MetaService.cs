using System.Linq;
using Ardalis.SmartEnum;

namespace Catalog.Domain.Enum
{
    public sealed class MetaService : SmartEnum<MetaService>
    {
        public static MetaService ListMethods = new MetaService("listMethods", 1);
        public static MetaService GetWsdl = new MetaService("getWsdl", 2);

        public static MetaService GetSecurityServerOperationalData =
            new MetaService("getSecurityServerOperationalData", 2);

        public static MetaService GetSecurityServerMetrics = new MetaService("getSecurityServerMetrics", 2);
        public static MetaService GetSecurityServerHealthData = new MetaService("getSecurityServerHealthData", 2);
        private MetaService(string name, int value) : base(name, value) { }

        public static bool IsMetaService(string name) => List.Any(service => service.Name.Equals(name));
    }
}