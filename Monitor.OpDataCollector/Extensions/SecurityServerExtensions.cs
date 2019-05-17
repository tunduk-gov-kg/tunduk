using Monitor.Domain.Entity;
using XRoad.Domain;

namespace Monitor.OpDataCollector.Extensions
{
    public static class SecurityServerExtensions
    {
        public static SecurityServerIdentifier GetIdentifier(this Server server)
        {
            return new SecurityServerIdentifier
            {
                Instance = server.Instance,
                MemberClass = server.MemberClass,
                MemberCode = server.MemberCode,
                SecurityServerCode = server.Code
            };
        }
    }
}