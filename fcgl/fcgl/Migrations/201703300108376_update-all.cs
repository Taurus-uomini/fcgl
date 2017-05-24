namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateall : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderModels", "houseProperty_id", "dbo.HousePropertyModels");
            DropForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels");
            DropIndex("dbo.HousePropertyModels", new[] { "owner_id" });
            DropIndex("dbo.OrderModels", new[] { "houseProperty_id" });
            AlterColumn("dbo.HousePropertyModels", "owner_id", c => c.Int(nullable: false));
            AlterColumn("dbo.OrderModels", "houseProperty_id", c => c.Int(nullable: false));
            CreateIndex("dbo.HousePropertyModels", "owner_id");
            CreateIndex("dbo.OrderModels", "houseProperty_id");
            AddForeignKey("dbo.OrderModels", "houseProperty_id", "dbo.HousePropertyModels", "id", cascadeDelete: true);
            AddForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels");
            DropForeignKey("dbo.OrderModels", "houseProperty_id", "dbo.HousePropertyModels");
            DropIndex("dbo.OrderModels", new[] { "houseProperty_id" });
            DropIndex("dbo.HousePropertyModels", new[] { "owner_id" });
            AlterColumn("dbo.OrderModels", "houseProperty_id", c => c.Int());
            AlterColumn("dbo.HousePropertyModels", "owner_id", c => c.Int());
            CreateIndex("dbo.OrderModels", "houseProperty_id");
            CreateIndex("dbo.HousePropertyModels", "owner_id");
            AddForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels", "id");
            AddForeignKey("dbo.OrderModels", "houseProperty_id", "dbo.HousePropertyModels", "id");
        }
    }
}
