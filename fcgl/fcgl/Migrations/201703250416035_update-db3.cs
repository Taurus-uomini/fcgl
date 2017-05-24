namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderModels", "UserModels_id", c => c.Int());
            AddColumn("dbo.OrderModels", "seller_id", c => c.Int());
            CreateIndex("dbo.OrderModels", "UserModels_id");
            CreateIndex("dbo.OrderModels", "seller_id");
            AddForeignKey("dbo.OrderModels", "UserModels_id", "dbo.UserModels", "id");
            AddForeignKey("dbo.OrderModels", "seller_id", "dbo.UserModels", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderModels", "seller_id", "dbo.UserModels");
            DropForeignKey("dbo.OrderModels", "UserModels_id", "dbo.UserModels");
            DropIndex("dbo.OrderModels", new[] { "seller_id" });
            DropIndex("dbo.OrderModels", new[] { "UserModels_id" });
            DropColumn("dbo.OrderModels", "seller_id");
            DropColumn("dbo.OrderModels", "UserModels_id");
        }
    }
}
