using System.Linq;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.Domain.Helpers
{
    public static class CatalogDbContextHelpers
    {
        public static Member FindMember(this IQueryable<Member> members, SubSystemIdentifier identifier)
        {
            return members.SingleOrDefault(member =>
                member.Instance == identifier.Instance
                && member.MemberClass == identifier.MemberClass
                && member.MemberCode == identifier.MemberCode);
        }

        public static SubSystem FindSubSystem(this IQueryable<SubSystem> subSystems, SubSystemIdentifier identifier)
        {
            return subSystems.Include(subSystem => subSystem.Member)
                .FirstOrDefault(subSystem =>
                    subSystem.Member.Instance == identifier.Instance
                    && subSystem.Member.MemberClass == identifier.MemberClass
                    && subSystem.Member.MemberCode == identifier.MemberCode
                    && subSystem.SubSystemCode == identifier.SubSystemCode);
        }

        public static Service FindService(this IQueryable<Service> services, ServiceIdentifier identifier)
        {
            return services.Include(service => service.SubSystem)
                .ThenInclude(subSystem => subSystem.Member)
                .FirstOrDefault(service =>
                    service.SubSystem.Member.Instance == identifier.Instance
                    && service.SubSystem.Member.MemberClass == identifier.MemberClass
                    && service.SubSystem.Member.MemberCode == identifier.MemberCode
                    && service.SubSystem.SubSystemCode == identifier.SubSystemCode
                    && service.ServiceCode == identifier.ServiceCode
                    && service.ServiceVersion == identifier.ServiceVersion
                );
        }
    }
}