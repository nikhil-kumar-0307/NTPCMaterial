namespace NTPCMaterial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameAmountToQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaterialPurchases", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.MaterialPurchases", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MaterialPurchases", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.MaterialPurchases", "Quantity");
        }
    }
}
