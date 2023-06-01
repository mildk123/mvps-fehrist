namespace fehrist.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TASKS")]
    public partial class TASK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TASK()
        {
            CHECKLISTs = new HashSet<CHECKLIST>();
            TASK_IMAGES = new HashSet<TASK_IMAGES>();
        }

        public int TASKID { get; set; }

        public int? ACCOUNTID { get; set; }

        [Column(TypeName = "text")]
        public string T_TITLE { get; set; }

        [Column(TypeName = "text")]
        public string T_DESC { get; set; }

        [StringLength(50)]
        public string T_STATUS { get; set; }

        [StringLength(50)]
        public string T_COLOR { get; set; }

        [StringLength(50)]
        public string T_DUE_DATE_TIME { get; set; }

        [StringLength(50)]
        public string T_ADDED_DATE_TIME { get; set; }

        [StringLength(50)]
        public string CREATED_BY { get; set; }

        public DateTime? DATE_CREATED { get; set; }

        [StringLength(50)]
        public string UPDATE_BY { get; set; }

        public DateTime? DATE_UPDATED { get; set; }

        public virtual ACCOUNT ACCOUNT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHECKLIST> CHECKLISTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASK_IMAGES> TASK_IMAGES { get; set; }
    }
}
