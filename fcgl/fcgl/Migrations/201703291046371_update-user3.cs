namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateuser3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserModels", "idcard", c => c.String(nullable: false));
            AddColumn("dbo.UserModels", "adress", c => c.String(nullable: false));
            AddColumn("dbo.UserModels", "area_id", c => c.Int());
            AlterColumn("dbo.UserModels", "relname", c => c.String(nullable: false));
            AlterColumn("dbo.UserModels", "email", c => c.String(nullable: false));
            CreateIndex("dbo.UserModels", "area_id");
            AddForeignKey("dbo.UserModels", "area_id", "dbo.AreasModels", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserModels", "area_id", "dbo.AreasModels");
            DropIndex("dbo.UserModels", new[] { "area_id" });
            AlterColumn("dbo.UserModels", "email", c => c.String());
            AlterColumn("dbo.UserModels", "relname", c => c.String());
            DropColumn("dbo.UserModels", "area_id");
            DropColumn("dbo.UserModels", "adress");
            DropColumn("dbo.UserModels", "idcard");
        }
    }
}
