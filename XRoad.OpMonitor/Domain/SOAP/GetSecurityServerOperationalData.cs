using System.Xml.Serialization;

namespace XRoad.OpMonitor.Domain.SOAP
{
    [XmlRoot(ElementName = "getSecurityServerOperationalData", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd")]
    public class GetSecurityServerOperationalData
    {
        [XmlElement(ElementName = "searchCriteria", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd",
            IsNullable = false)]
        public SearchCriteria SearchCriteria { get; set; }

        [XmlElement(ElementName = "outputSpec", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd",
            IsNullable = true)]
        public OutputSpecification OutputSpecification { get; set; }

        [XmlIgnore] public bool OutputSpecificationSpecified => null != OutputSpecification;
    }
}