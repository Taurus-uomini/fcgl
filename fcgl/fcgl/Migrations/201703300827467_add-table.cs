namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PropertyImageModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        url = c.String(),
                        houseProperty_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.HousePropertyModels", t => t.houseProperty_id, cascadeDelete: true)
                .Index(t => t.houseProperty_id);
            
            AddColumn("dbo.HousePropertyModels", "propertyUrl1", c => c.String());
            AddColumn("dbo.HousePropertyModels", "propertyUrl2", c => c.String());
            AddColumn("dbo.HousePropertyModels", "propertyUrl3", c => c.String());
            DropColumn("dbo.HousePropertyModels", "propertyUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HousePropertyModels", "propertyUrl", c => c.String());
            DropForeignKey("dbo.PropertyImageModels", "houseProperty_id", "dbo.HousePropertyModels");
            DropIndex("dbo.PropertyImageModels", new[] { "houseProperty_id" });
            DropColumn("dbo.HousePropertyModels", "propertyUrl3");
            DropColumn("dbo.HousePropertyModels", "propertyUrl2");
            DropColumn("dbo.HousePropertyModels", "propertyUrl1");
            DropTable("dbo.PropertyImageModels");
        }
    }
}
