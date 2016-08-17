namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsIntervalMove : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "IsInternalMove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContainerTrackings", "IsInternalMove");
        }
    }
}
