using XRoad.Domain.Header;

namespace XRoad.Domain
{
    public class ServiceIdentifier : SubSystemIdentifier
    {
        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }

        public static implicit operator ServiceIdentifier(XRoadService xRoadService)
        {
            return new ServiceIdentifier
            {
                Instance = xRoadService.Instance,
                MemberClass = xRoadService.MemberClass,
                MemberCode = xRoadService.MemberCode,
                SubSystemCode = xRoadService.SubSystemCode,
                ServiceCode = xRoadService.ServiceCode,
                ServiceVersion = xRoadService.ServiceVersion
            };
        }
    }
}