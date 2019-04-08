using System.Collections.Generic;
using System.Xml.Serialization;

namespace XRoad.OpMonitor.Domain.SOAP {
    [XmlRoot(ElementName = "outputSpec", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd")]
    public class OutputSpecification {
        [XmlElement(ElementName = "outputField")]
        public List<string> OutputFields { get; set; }
    }
}