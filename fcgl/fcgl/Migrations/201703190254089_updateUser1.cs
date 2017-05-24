namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserModels", "relname", c => c.String());
            AddColumn("dbo.UserModels", "email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserModels", "email");
            DropColumn("dbo.UserModels", "relname");
        }
    }
}
