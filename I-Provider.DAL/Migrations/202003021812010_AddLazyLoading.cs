namespace I_Provider.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLazyLoading : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TariffTypes", "TariffPlan_Id", "dbo.TariffPlans");
            DropIndex("dbo.TariffTypes", new[] { "TariffPlan_Id" });
            CreateTable(
                "dbo.TariffTypeTariffPlans",
                c => new
                    {
                        TariffType_Id = c.Int(nullable: false),
                        TariffPlan_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TariffType_Id, t.TariffPlan_Id })
                .ForeignKey("dbo.TariffTypes", t => t.TariffType_Id, cascadeDelete: true)
                .ForeignKey("dbo.TariffPlans", t => t.TariffPlan_Id, cascadeDelete: true)
                .Index(t => t.TariffType_Id)
                .Index(t => t.TariffPlan_Id);
            
            DropColumn("dbo.TariffTypes", "TariffPlan_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TariffTypes", "TariffPlan_Id", c => c.Int());
            DropForeignKey("dbo.TariffTypeTariffPlans", "TariffPlan_Id", "dbo.TariffPlans");
            DropForeignKey("dbo.TariffTypeTariffPlans", "TariffType_Id", "dbo.TariffTypes");
            DropIndex("dbo.TariffTypeTariffPlans", new[] { "TariffPlan_Id" });
            DropIndex("dbo.TariffTypeTariffPlans", new[] { "TariffType_Id" });
            DropTable("dbo.TariffTypeTariffPlans");
            CreateIndex("dbo.TariffTypes", "TariffPlan_Id");
            AddForeignKey("dbo.TariffTypes", "TariffPlan_Id", "dbo.TariffPlans", "Id");
        }
    }
}
