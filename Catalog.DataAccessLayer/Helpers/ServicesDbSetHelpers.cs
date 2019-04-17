using System.Linq;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.DataAccessLayer.Helpers
{
    public static class ServicesDbSetHelpers
    {
        public static Service FindByServiceIdentifier(this IQueryable<Service> source,
            ServiceIdentifier serviceIdentifier)
        {
            return source
                .Include(service => service.SubSystem)
                .ThenInclude(subSystem => subSystem.Member)
                .SingleOrDefault(service =>
                    service.ServiceCode == serviceIdentifier.ServiceCode
                    && service.ServiceVersion == serviceIdentifier.ServiceVersion
                    && service.SubSystem.SubSystemCode == serviceIdentifier.SubSystemCode
                    && service.SubSystem.Member.Instance == serviceIdentifier.Instance
                    && service.SubSystem.Member.MemberClass == serviceIdentifier.MemberClass
                    && service.SubSystem.Member.MemberCode == serviceIdentifier.MemberCode
                );
        }
    }
}