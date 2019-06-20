using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace XRoad.OpMonitor.Domain
{
    [XmlRoot(ElementName = "getSecurityServerOperationalDataResponse",
        Namespace = "http://x-road.eu/xsd/op-monitoring.xsd")]
    public class OperationalData
    {
        [XmlElement(ElementName = "recordsCount", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd",
            IsNullable = false)]
        public long RecordsCount { get; set; }

        /// <summary>
        /// Unix timestamp in seconds to use for field
        /// recordsFrom of the next query. This element is present in case
        /// the size of the response has been limited or the timestamp of
        /// the field recordsTo was in the future.
        /// </summary>
        [XmlElement(ElementName = "nextRecordsFrom", Namespace = "http://x-road.eu/xsd/op-monitoring.xsd")]
        public long? NextRecordsFrom { get; set; }

        [XmlIgnore] public bool NextRecordsFromSpecified => NextRecordsFrom != null;
        [XmlIgnore] public byte[] Attachment { get; set; }

        public OperationalDataRecordDto[] Records
        {
            get
            {
                using (var stream = new GZipStream(new MemoryStream(Attachment), CompressionMode.Decompress))
                {
                    var streamReader = new StreamReader(stream);
                    var jsonString = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<OperationalDataRecordCollection>(jsonString).Records;
                }
            }
        }

        public class OperationalDataRecordCollection
        {
            public OperationalDataRecordDto[] Records { get; set; }
        }
    }
}