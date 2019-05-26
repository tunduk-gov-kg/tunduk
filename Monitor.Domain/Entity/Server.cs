using System;

namespace Monitor.Domain.Entity
{
    public class Server
    {
        public long Id { get; set; }
        public string Instance { get; set; }
        public string MemberClass { get; set; }
        public string MemberCode { get; set; }
        public string Code { get; set; }
        public DateTime NextRecordsFrom { get; set; }
    }
}