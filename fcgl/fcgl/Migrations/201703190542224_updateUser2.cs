namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HousePropertyModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        adress = c.String(nullable: false),
                        prize = c.Single(nullable: false),
                        detial = c.String(nullable: false),
                        propertyUrl = c.String(),
                        owner_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.UserModels", t => t.owner_id)
                .Index(t => t.owner_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HousePropertyModels", "owner_id", "dbo.UserModels");
            DropIndex("dbo.HousePropertyModels", new[] { "owner_id" });
            DropTable("dbo.HousePropertyModels");
        }
    }
}
