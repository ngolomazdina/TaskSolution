using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TaskWebApp.Models
{
    public class Task
    {
        public Guid? Uid { get; set; }
        public Guid? ParentUid { get; set; }
        public string ParentName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public Guid ClientUid { get; set; }
        public string ClientName { get; set; }
        public int PlanTime { get; set; }
        public int FactTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> Executors { get; set; }
    }
}