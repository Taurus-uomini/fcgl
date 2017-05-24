namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfoModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        webName = c.String(nullable: false),
                        designer = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InfoModels");
        }
    }
}
