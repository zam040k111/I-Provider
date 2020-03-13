namespace I_Provider.BLL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Action", c => c.String());
            AddColumn("dbo.Logs", "Number", c => c.Int(nullable: false));
            AddColumn("dbo.Logs", "Field", c => c.String());
            AddColumn("dbo.Logs", "Value", c => c.String());
            DropColumn("dbo.Logs", "Type");
            DropColumn("dbo.Logs", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "Description", c => c.String());
            AddColumn("dbo.Logs", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Logs", "Value");
            DropColumn("dbo.Logs", "Field");
            DropColumn("dbo.Logs", "Number");
            DropColumn("dbo.Logs", "Action");
        }
    }
}
