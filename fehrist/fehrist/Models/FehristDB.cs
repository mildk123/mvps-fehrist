namespace fehrist.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBEntities : DbContext
    {
        public DBEntities()
            : base("name=DBEntities")
        {
        }

        public virtual DbSet<ACCOUNT> ACCOUNTS { get; set; }
        public virtual DbSet<CHECKLIST> CHECKLISTs { get; set; }
        public virtual DbSet<ROLE> ROLES { get; set; }
        public virtual DbSet<TASK_IMAGES> TASK_IMAGES { get; set; }
        public virtual DbSet<TASK> TASKS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.PASS)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.AC_STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<ACCOUNT>()
                .Property(e => e.UPDATE_BY)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKLIST>()
                .Property(e => e.CL_DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKLIST>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKLIST>()
                .Property(e => e.UPDATE_BY)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE>()
                .Property(e => e.UPDATE_BY)
                .IsUnicode(false);

            modelBuilder.Entity<TASK_IMAGES>()
                .Property(e => e.TI_PATH)
                .IsUnicode(false);

            modelBuilder.Entity<TASK_IMAGES>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<TASK_IMAGES>()
                .Property(e => e.UPDATE_BY)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.T_TITLE)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.T_DESC)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.T_STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.T_COLOR)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.T_DUE_DATE_TIME)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.T_ADDED_DATE_TIME)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.CREATED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<TASK>()
                .Property(e => e.UPDATE_BY)
                .IsUnicode(false);
        }
    }
}
