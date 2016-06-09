namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "ContainerStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContainerTrackings", "ContainerStatus");
        }
    }
}
