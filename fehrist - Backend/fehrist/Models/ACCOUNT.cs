namespace fehrist.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ACCOUNTS")]
    public partial class ACCOUNT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ACCOUNT()
        {
            TASKS = new HashSet<TASK>();
        }

        public int ACCOUNTID { get; set; }

        public int? ROLEID { get; set; }

        [Required]
        [StringLength(50)]
        public string NAME { get; set; }

        [Required]
        [StringLength(50)]
        public string EMAIL { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string PASS { get; set; }

        [StringLength(50)]
        public string AC_STATUS { get; set; }

        [StringLength(50)]
        public string CREATED_BY { get; set; }

        public DateTime? DATE_CREATED { get; set; }

        [StringLength(50)]
        public string UPDATE_BY { get; set; }

        public DateTime? DATE_UPDATED { get; set; }

        public virtual ROLE ROLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TASK> TASKS { get; set; }
    }
}
