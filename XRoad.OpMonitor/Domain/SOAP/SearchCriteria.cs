using System.Xml.Serialization;
using XRoad.Domain.Header;

namespace XRoad.OpMonitor.Domain.SOAP
{
    [XmlRoot(ElementName = "searchCriteria", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd")]
    public class SearchCriteria
    {
        /// <summary>
        ///     The beginning of the time interval of requested operational data (Unix timestamp in seconds)
        /// </summary>
        [XmlElement(ElementName = "recordsFrom", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd",
            IsNullable = false)]
        public long RecordsFrom { get; set; }

        /// <summary>
        ///     The end of the time interval of requested operational data (Unix timestamp in seconds)
        /// </summary>
        [XmlElement(ElementName = "recordsTo", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd",
            IsNullable = false)]
        public long RecordsTo { get; set; }

        [XmlElement(ElementName = "client", IsNullable = true, Namespace = "http://x-road.eu/xsd/xroad.xsd")]
        public XRoadClient Client { get; set; }

        [XmlIgnore] public bool ClientSpecified => null != Client;
    }
}