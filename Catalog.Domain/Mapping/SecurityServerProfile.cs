using AutoMapper;
using Catalog.Domain.Entity;
using XRoad.Domain;

namespace Catalog.Domain.Mapping
{
    public class SecurityServerProfile : Profile
    {
        public SecurityServerProfile()
        {
            CreateMap<SecurityServer, SecurityServerIdentifier>()
                .ForMember(
                    identifier => identifier.Instance,
                    expression => expression.MapFrom(entity => entity.Member.Instance))
                .ForMember(
                    identifier => identifier.MemberClass,
                    expression => expression.MapFrom(entity => entity.Member.MemberClass))
                .ForMember(
                    identifier => identifier.MemberCode,
                    expression => expression.MapFrom(entity => entity.Member.MemberCode))
                .ForMember(
                    identifier => identifier.SecurityServerCode,
                    expression => expression.MapFrom(entity => entity.SecurityServerCode));
        }
    }
}