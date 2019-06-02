using System;
using Monitor.Domain.Entity;
using XRoad.Domain;

namespace Monitor.Domain
{
    public static class ServerExtensions
    {
        public static bool SameAs(this Server server, SecurityServerData securityServerData)
        {
            var identifier = securityServerData.SecurityServerIdentifier;
            return server.Instance.Equals(identifier.Instance)
                   && server.MemberClass.Equals(identifier.MemberClass)
                   && server.MemberCode.Equals(identifier.MemberCode)
                   && server.Code.Equals(identifier.SecurityServerCode);
        }

        public static bool SameAs(this SecurityServerData securityServerData, Server entity)
        {
            var identifier = securityServerData.SecurityServerIdentifier;
            return identifier.Instance.Equals(entity.Instance)
                   && identifier.MemberClass.Equals(entity.MemberClass)
                   && identifier.MemberCode.Equals(entity.MemberCode)
                   && identifier.SecurityServerCode.Equals(entity.Code);
        }

        public static Server AsEntity(this SecurityServerData securityServerData)
        {
            var identifier = securityServerData.SecurityServerIdentifier;
            return new Server
            {
                Instance = identifier.Instance,
                MemberClass = identifier.MemberClass,
                MemberCode = identifier.MemberCode,
                Code = identifier.SecurityServerCode,
                NextRecordsFrom = new DateTime(1970, 1, 1)
            };
        }
    }
}