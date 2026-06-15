namespace NTPCMaterial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MAterialRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaterialRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestNumber = c.String(nullable: false, maxLength: 30),
                        MaterialPurchaseId = c.Int(nullable: false),
                        MaterialCode = c.String(nullable: false, maxLength: 50),
                        MaterialName = c.String(nullable: false, maxLength: 200),
                        QuantityRequested = c.Int(nullable: false),
                        Department = c.String(nullable: false, maxLength: 100),
                        RequiredByDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Remarks = c.String(maxLength: 500),
                        Status = c.String(nullable: false, maxLength: 20),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RequestedBy = c.String(nullable: false, maxLength: 50),
                        ReviewedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        ReviewedBy = c.String(maxLength: 50),
                        ReviewRemarks = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MaterialPurchases", t => t.MaterialPurchaseId, cascadeDelete: true)
                .Index(t => t.MaterialPurchaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaterialRequests", "MaterialPurchaseId", "dbo.MaterialPurchases");
            DropIndex("dbo.MaterialRequests", new[] { "MaterialPurchaseId" });
            DropTable("dbo.MaterialRequests");
        }
    }
}
