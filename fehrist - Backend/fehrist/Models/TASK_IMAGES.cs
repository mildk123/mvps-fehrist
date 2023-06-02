namespace fehrist.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TASK_IMAGES
    {
        [Key]
        public int IMAGEID { get; set; }

        [Column(TypeName = "text")]
        public string TI_PATH { get; set; }

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
