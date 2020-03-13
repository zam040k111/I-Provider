namespace I_Provider.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerTariffPlanAndType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.UserId_Id);
            
            CreateTable(
                "dbo.TariffPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ShortDesc = c.String(),
                        Price = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        DiscWillEnd = c.DateTime(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.TariffTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        NumberChannels = c.Int(nullable: false),
                        Speed = c.Int(nullable: false),
                        DiscountPrice = c.Double(nullable: false),
                        NumberHD = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        TariffPlan_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TariffPlans", t => t.TariffPlan_Id)
                .Index(t => t.TariffPlan_Id);
            
            AddColumn("dbo.AspNetUsers", "TariffPlan_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "TariffPlan_Id");
            AddForeignKey("dbo.AspNetUsers", "TariffPlan_Id", "dbo.TariffPlans", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TariffPlans", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.AspNetUsers", "TariffPlan_Id", "dbo.TariffPlans");
            DropForeignKey("dbo.TariffTypes", "TariffPlan_Id", "dbo.TariffPlans");
            DropIndex("dbo.AspNetUsers", new[] { "TariffPlan_Id" });
            DropIndex("dbo.TariffTypes", new[] { "TariffPlan_Id" });
            DropIndex("dbo.TariffPlans", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", new[] { "UserId_Id" });
            DropColumn("dbo.AspNetUsers", "TariffPlan_Id");
            DropTable("dbo.TariffTypes");
            DropTable("dbo.TariffPlans");
            DropTable("dbo.Customers");
        }
    }
}
