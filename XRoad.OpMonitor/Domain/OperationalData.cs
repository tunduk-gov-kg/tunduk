using System.Xml.Serialization;

namespace XRoad.OpMonitor.Domain {
    [XmlRoot(ElementName = "getSecurityServerOperationalDataResponse",
        Namespace = "http://x-road.eu/xsd/op-monitoring.xsd")]
    public class OperationalData {
        [XmlElement(ElementName = "recordsCount", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd",
            IsNullable = false)]
        public long RecordsCount { get; set; }

        [XmlElement(ElementName = "nextRecordsFrom", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd")]
        public long? NextRecordsFrom { get; set; }

        [XmlIgnore] public bool NextRecordsFromSpecified => NextRecordsFrom != null;
        [XmlIgnore] public byte[] Attachment { get; set; }
    }
}