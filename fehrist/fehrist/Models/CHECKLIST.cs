namespace fehrist.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHECKLIST")]
    public partial class CHECKLIST
    {
        [Key]
        public int CHECKID { get; set; }

        [Column(TypeName = "text")]
        public string CL_DESCRIPTION { get; set; }

        public int? TASKID { get; set; }

        [StringLength(50)]
        public string CREATED_BY { get; set; }

        public DateTime? DATE_CREATED { get; set; }

        [StringLength(50)]
        public string UPDATE_BY { get; set; }

        public DateTime? DATE_UPDATED { get; set; }

        public virtual TASK TASK { get; set; }
    }
}
