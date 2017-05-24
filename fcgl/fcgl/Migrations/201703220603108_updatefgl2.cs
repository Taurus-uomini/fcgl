namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefgl2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CitiesModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        cityid = c.String(nullable: false),
                        city = c.String(nullable: false),
                        provinceid = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProvincesModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        provinceid = c.String(nullable: false),
                        province = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProvincesModels");
            DropTable("dbo.CitiesModels");
        }
    }
}
