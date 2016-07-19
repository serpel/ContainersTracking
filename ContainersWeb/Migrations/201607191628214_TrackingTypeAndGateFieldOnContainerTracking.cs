namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackingTypeAndGateFieldOnContainerTracking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "TrackingType", c => c.Int(nullable: false));
            AddColumn("dbo.ContainerTrackings", "GateId", c => c.Int());
            AddColumn("dbo.ContainerTrackings", "Gate_RegionId", c => c.Int());
            CreateIndex("dbo.ContainerTrackings", "Gate_RegionId");
            AddForeignKey("dbo.ContainerTrackings", "Gate_RegionId", "dbo.Regions", "RegionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContainerTrackings", "Gate_RegionId", "dbo.Regions");
            DropIndex("dbo.ContainerTrackings", new[] { "Gate_RegionId" });
            DropColumn("dbo.ContainerTrackings", "Gate_RegionId");
            DropColumn("dbo.ContainerTrackings", "GateId");
            DropColumn("dbo.ContainerTrackings", "TrackingType");
        }
    }
}
