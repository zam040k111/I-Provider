namespace I_Provider.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTariffPlan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "TariffPlan_Id", "dbo.TariffPlans");
            DropForeignKey("dbo.TariffPlans", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.TariffPlans", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "TariffPlan_Id" });
            CreateTable(
                "dbo.TariffPlanCustomers",
                c => new
                    {
                        TariffPlan_Id = c.Int(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TariffPlan_Id, t.Customer_Id })
                .ForeignKey("dbo.TariffPlans", t => t.TariffPlan_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.TariffPlan_Id)
                .Index(t => t.Customer_Id);
            
            DropColumn("dbo.TariffPlans", "Customer_Id");
            DropColumn("dbo.AspNetUsers", "TariffPlan_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "TariffPlan_Id", c => c.Int());
            AddColumn("dbo.TariffPlans", "Customer_Id", c => c.Int());
            DropForeignKey("dbo.TariffPlanCustomers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.TariffPlanCustomers", "TariffPlan_Id", "dbo.TariffPlans");
            DropIndex("dbo.TariffPlanCustomers", new[] { "Customer_Id" });
            DropIndex("dbo.TariffPlanCustomers", new[] { "TariffPlan_Id" });
            DropTable("dbo.TariffPlanCustomers");
            CreateIndex("dbo.AspNetUsers", "TariffPlan_Id");
            CreateIndex("dbo.TariffPlans", "Customer_Id");
            AddForeignKey("dbo.TariffPlans", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.AspNetUsers", "TariffPlan_Id", "dbo.TariffPlans", "Id");
        }
    }
}
