namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        status = c.Int(nullable: false),
                        buyer_id = c.Int(),
                        houseProperty_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.UserModels", t => t.buyer_id)
                .ForeignKey("dbo.HousePropertyModels", t => t.houseProperty_id)
                .Index(t => t.buyer_id)
                .Index(t => t.houseProperty_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderModels", "houseProperty_id", "dbo.HousePropertyModels");
            DropForeignKey("dbo.OrderModels", "buyer_id", "dbo.UserModels");
            DropIndex("dbo.OrderModels", new[] { "houseProperty_id" });
            DropIndex("dbo.OrderModels", new[] { "buyer_id" });
            DropTable("dbo.OrderModels");
        }
    }
}
