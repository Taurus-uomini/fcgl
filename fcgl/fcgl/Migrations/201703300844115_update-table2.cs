namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels");
            DropIndex("dbo.HousePropertyModels", new[] { "owner_id" });
            AlterColumn("dbo.HousePropertyModels", "owner_id", c => c.Int());
            CreateIndex("dbo.HousePropertyModels", "owner_id");
            AddForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels");
            DropIndex("dbo.HousePropertyModels", new[] { "owner_id" });
            AlterColumn("dbo.HousePropertyModels", "owner_id", c => c.Int(nullable: false));
            CreateIndex("dbo.HousePropertyModels", "owner_id");
            AddForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels", "id", cascadeDelete: true);
        }
    }
}
