//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskWcf.Model.DB
{
    using System;
    
    public partial class sp_GetTaskList_Result
    {
        public System.Guid Uid { get; set; }
        public Nullable<System.Guid> ParentUid { get; set; }
        public string ParentName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.Guid ClientUid { get; set; }
        public string ClientName { get; set; }
        public Nullable<int> PlanTime { get; set; }
        public Nullable<int> FactTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}