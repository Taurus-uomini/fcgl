namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderModels", "startTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderModels", "cancelTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.OrderModels", "finishTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderModels", "finishTime");
            DropColumn("dbo.OrderModels", "cancelTime");
            DropColumn("dbo.OrderModels", "startTime");
        }
    }
}
