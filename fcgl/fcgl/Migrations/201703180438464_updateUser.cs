namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserModels", "phone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserModels", "phone");
        }
    }
}
