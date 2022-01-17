using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskWebApp.Models.Mappings
{
    public static class MappingTreeNode
    {
        public static List<TaskNode> LoadTreeNodes(List<TaskWcf.Model.DTO.Task> tasks)
        {
            var result = new List<TaskNode>();

            foreach (var tn in tasks.Where(t=>t.ParentUid==null).OrderBy(t=>t.Name))
            {
                var chNodes= tasks.Where(ct => ct.ParentUid == tn.Uid).ToList();
                if (chNodes.Count == 0)
                {
                    var node = new TaskNode();
                    node.text = tn.Name;
                    node.tags.Add(tn.Uid.ToString());
                    result.Add(node);
                }
                else
                {
                    var nodeCh = new TaskNodeWithChildren();
                    nodeCh.text = tn.Name;
                    nodeCh.tags.Add(tn.Uid.ToString());
                    nodeCh.nodes = GetChildNodes(chNodes, tasks);
                    result.Add(nodeCh);
                }    

            }
            return result;
        }

        static List<TaskNode> GetChildNodes(List<TaskWcf.Model.DTO.Task> tasks, List<TaskWcf.Model.DTO.Task> tree)
        {
            var result = new List<TaskNode>();

            foreach (var task in tasks)
            {
                foreach (var tn in tasks.Where(t => t.Uid == task.Uid))
                {
                    var chNodes = tree.Where(ct => ct.ParentUid == tn.Uid).ToList();
                    if (chNodes.Count == 0)
                    {
                        var node = new TaskNode();
                        node.text = tn.Name;
                        node.tags.Add(tn.Uid.ToString());
                        result.Add(node);
                    }
                    else
                    {
                        var nodeCh = new TaskNodeWithChildren();
                        nodeCh.text = tn.Name;
                        nodeCh.tags.Add(tn.Uid.ToString());
                        nodeCh.nodes = GetChildNodes(chNodes,tree);
                        result.Add(nodeCh);
                    }
                }
            }           
            return result;
        }
    }
}