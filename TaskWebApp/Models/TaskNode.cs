using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskWebApp.Models
{
    [Serializable]
    public class TaskNode
    {
        public List<string> tags { get; set; } = new List<string>();
        public string text { get; set; }        
    }

    public class TaskNodeWithChildren : TaskNode
    {
        public List<TaskNode> nodes { get; set; } = new List<TaskNode>();
    }
}