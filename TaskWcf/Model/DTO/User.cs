using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TaskWcf.Model.DTO
{
    [DataContract]
    public class User
    {
        [DataMember]
        public Guid Uid { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}