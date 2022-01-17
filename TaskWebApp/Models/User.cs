using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TaskWebApp.Models
{
    public class User
    {
        public Guid Uid { get; set; }
        public string Name { get; set; }
    }
}