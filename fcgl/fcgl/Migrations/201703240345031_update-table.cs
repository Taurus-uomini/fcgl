namespace fcgl.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HousePropertyModels", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HousePropertyModels", "status");
        }
    }
}
