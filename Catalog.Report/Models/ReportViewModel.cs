using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Report.Models
{
    public class ReportViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<Member> Members { get; set; }
    }

    public class Member
    {
        public string Name { get; set; }
        public List<SubSystem> SubSystems { get; set; }
        public List<ProducedService> MetaServices { get; set; }

        public int HandledRequestsCount
        {
            get { return SubSystems.Sum(system => system.ProducedServices.Sum(service => service.HandledRequests)); }
        }
    }

    public class SubSystem
    {
        public string Name { get; set; }
        public List<ProducedService> ProducedServices { get; set; }
    }

    public class ProducedService
    {
        public bool IsMetaService { get; set; }
        public string Name { get; set; }
        public int HandledRequests { get; set; }
    }
}