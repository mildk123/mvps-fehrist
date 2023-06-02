namespace fehrist.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removephonecolumnfromaccounts : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ACCOUNTS", "PHONE");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ACCOUNTS", "PHONE", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
