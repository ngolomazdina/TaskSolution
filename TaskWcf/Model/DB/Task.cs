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
    using System.Collections.Generic;
    
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.Task1 = new HashSet<Task>();
            this.TaskExecutor = new HashSet<TaskExecutor>();
        }
    
        public System.Guid Uid { get; set; }
        public Nullable<System.Guid> ParentUid { get; set; }
        public int StateId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.Guid ClientUid { get; set; }
        public Nullable<int> PlanTime { get; set; }
        public Nullable<int> FactTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsArchive { get; set; }
    
        public virtual State State { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Task1 { get; set; }
        public virtual Task Task2 { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskExecutor> TaskExecutor { get; set; }
    }
}