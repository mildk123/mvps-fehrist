//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace fehrist.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ACCOUNT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACCOUNT()
        {
            this.TASKS = new HashSet<TASK>();
        }
    
        public int ACCOUNTID { get; set; }
        public Nullable<int> ROLEID { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string PASS { get; set; }
        public string PHONE { get; set; }
        public string AC_STATUS { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> DATE_CREATED { get; set; }
        public string UPDATE_BY { get; set; }
        public Nullable<System.DateTime> DATE_UPDATED { get; set; }
    
        public virtual ROLE ROLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASK> TASKS { get; set; }
    }
}
