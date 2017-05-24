namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefgl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreasModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        areaid = c.String(nullable: false),
                        area = c.String(nullable: false),
                        cityid = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.HousePropertyModels", "area_id", c => c.Int());
            CreateIndex("dbo.HousePropertyModels", "area_id");
            AddForeignKey("dbo.HousePropertyModels", "area_id", "dbo.AreasModels", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HousePropertyModels", "area_id", "dbo.AreasModels");
            DropIndex("dbo.HousePropertyModels", new[] { "area_id" });
            DropColumn("dbo.HousePropertyModels", "area_id");
            DropTable("dbo.AreasModels");
        }
    }
}
