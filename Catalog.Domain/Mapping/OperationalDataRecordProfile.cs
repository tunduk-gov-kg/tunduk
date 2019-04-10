using AutoMapper;
using Catalog.Domain.Entity;
using XRoad.OpMonitor.Domain;

namespace Catalog.Domain.Mapping
{
    public class OperationalDataRecordProfile : Profile
    {
        public OperationalDataRecordProfile()
        {
            CreateMap<OperationalDataRecordDto, OperationalDataRecord>();
        }
    }
}