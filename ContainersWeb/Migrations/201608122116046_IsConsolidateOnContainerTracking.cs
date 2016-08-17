namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsConsolidateOnContainerTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "IsConsolidate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContainerTrackings", "IsConsolidate");
        }
    }
}
