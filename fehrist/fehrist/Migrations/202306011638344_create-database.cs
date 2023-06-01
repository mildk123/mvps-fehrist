namespace fehrist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ACCOUNTS",
                c => new
                    {
                        ACCOUNTID = c.Int(nullable: false, identity: true),
                        ROLEID = c.Int(),
                        NAME = c.String(nullable: false, maxLength: 50, unicode: false),
                        EMAIL = c.String(nullable: false, maxLength: 50, unicode: false),
                        PASS = c.String(nullable: false, unicode: false, storeType: "text"),
                        PHONE = c.String(maxLength: 50, unicode: false),
                        AC_STATUS = c.String(maxLength: 50, unicode: false),
                        CREATED_BY = c.String(maxLength: 50, unicode: false),
                        DATE_CREATED = c.DateTime(),
                        UPDATE_BY = c.String(maxLength: 50, unicode: false),
                        DATE_UPDATED = c.DateTime(),
                    })
                .PrimaryKey(t => t.ACCOUNTID)
                .ForeignKey("dbo.ROLES", t => t.ROLEID)
                .Index(t => t.ROLEID);
            
            CreateTable(
                "dbo.ROLES",
                c => new
                    {
                        ROLEID = c.Int(nullable: false, identity: true),
                        NAME = c.String(nullable: false, maxLength: 50, unicode: false),
                        CREATED_BY = c.String(maxLength: 50, unicode: false),
                        DATE_CREATED = c.DateTime(),
                        UPDATE_BY = c.String(maxLength: 50, unicode: false),
                        DATE_UPDATED = c.DateTime(),
                    })
                .PrimaryKey(t => t.ROLEID);
            
            CreateTable(
                "dbo.TASKS",
                c => new
                    {
                        TASKID = c.Int(nullable: false, identity: true),
                        ACCOUNTID = c.Int(),
                        T_TITLE = c.String(unicode: false, storeType: "text"),
                        T_DESC = c.String(unicode: false, storeType: "text"),
                        T_STATUS = c.String(maxLength: 50, unicode: false),
                        T_COLOR = c.String(maxLength: 50, unicode: false),
                        T_DUE_DATE_TIME = c.String(maxLength: 50, unicode: false),
                        T_ADDED_DATE_TIME = c.String(maxLength: 50, unicode: false),
                        CREATED_BY = c.String(maxLength: 50, unicode: false),
                        DATE_CREATED = c.DateTime(),
                        UPDATE_BY = c.String(maxLength: 50, unicode: false),
                        DATE_UPDATED = c.DateTime(),
                    })
                .PrimaryKey(t => t.TASKID)
                .ForeignKey("dbo.ACCOUNTS", t => t.ACCOUNTID)
                .Index(t => t.ACCOUNTID);
            
            CreateTable(
                "dbo.CHECKLIST",
                c => new
                    {
                        CHECKID = c.Int(nullable: false, identity: true),
                        CL_DESCRIPTION = c.String(unicode: false, storeType: "text"),
                        TASKID = c.Int(),
                        CREATED_BY = c.String(maxLength: 50, unicode: false),
                        DATE_CREATED = c.DateTime(),
                        UPDATE_BY = c.String(maxLength: 50, unicode: false),
                        DATE_UPDATED = c.DateTime(),
                    })
                .PrimaryKey(t => t.CHECKID)
                .ForeignKey("dbo.TASKS", t => t.TASKID)
                .Index(t => t.TASKID);
            
            CreateTable(
                "dbo.TASK_IMAGES",
                c => new
                    {
                        IMAGEID = c.Int(nullable: false, identity: true),
                        TI_PATH = c.String(unicode: false, storeType: "text"),
                        TASKID = c.Int(),
                        CREATED_BY = c.String(maxLength: 50, unicode: false),
                        DATE_CREATED = c.DateTime(),
                        UPDATE_BY = c.String(maxLength: 50, unicode: false),
                        DATE_UPDATED = c.DateTime(),
                    })
                .PrimaryKey(t => t.IMAGEID)
                .ForeignKey("dbo.TASKS", t => t.TASKID)
                .Index(t => t.TASKID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TASK_IMAGES", "TASKID", "dbo.TASKS");
            DropForeignKey("dbo.CHECKLIST", "TASKID", "dbo.TASKS");
            DropForeignKey("dbo.TASKS", "ACCOUNTID", "dbo.ACCOUNTS");
            DropForeignKey("dbo.ACCOUNTS", "ROLEID", "dbo.ROLES");
            DropIndex("dbo.TASK_IMAGES", new[] { "TASKID" });
            DropIndex("dbo.CHECKLIST", new[] { "TASKID" });
            DropIndex("dbo.TASKS", new[] { "ACCOUNTID" });
            DropIndex("dbo.ACCOUNTS", new[] { "ROLEID" });
            DropTable("dbo.TASK_IMAGES");
            DropTable("dbo.CHECKLIST");
            DropTable("dbo.TASKS");
            DropTable("dbo.ROLES");
            DropTable("dbo.ACCOUNTS");
        }
    }
}
