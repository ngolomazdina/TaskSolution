using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TaskWcf.Model.DTO
{
    [DataContract]
    public class Task
    {
        [DataMember]
        public Guid? Uid { get; set; }

        [DataMember]
        public Guid? ParentUid { get; set; }

        [DataMember]
        public string ParentName { get; set; }

        [DataMember]
        public int StateId { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }
        
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        [DataMember]
        public Guid ClientUid { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public int PlanTime { get; set; }

        [DataMember]
        public int FactTime { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
        
    }
}