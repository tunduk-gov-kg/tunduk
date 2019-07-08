using System;
using System.ComponentModel.DataAnnotations.Schema;
using Monitor.Domain.Extensions;

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


        [NotMapped]
        public long NextRecordsFromTimestamp
        {
            get => NextRecordsFrom.ToSeconds();
            set => NextRecordsFrom = value.ToDateTime(TemporalType.Seconds);
        }
    }
}