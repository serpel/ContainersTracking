namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDriverIdFieldOnContainerTracking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContainerTrackings", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.ContainerTrackings", "SecuritySupervisorId", "dbo.SecuritySupervisors");
            DropIndex("dbo.ContainerTrackings", new[] { "DriverId" });
            DropIndex("dbo.ContainerTrackings", new[] { "SecuritySupervisorId" });
            AlterColumn("dbo.ContainerTrackings", "DriverId", c => c.Int());
            AlterColumn("dbo.ContainerTrackings", "SecuritySupervisorId", c => c.Int());
            CreateIndex("dbo.ContainerTrackings", "DriverId");
            CreateIndex("dbo.ContainerTrackings", "SecuritySupervisorId");
            AddForeignKey("dbo.ContainerTrackings", "DriverId", "dbo.Drivers", "DriverId");
            AddForeignKey("dbo.ContainerTrackings", "SecuritySupervisorId", "dbo.SecuritySupervisors", "SecuritySupervisorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContainerTrackings", "SecuritySupervisorId", "dbo.SecuritySupervisors");
            DropForeignKey("dbo.ContainerTrackings", "DriverId", "dbo.Drivers");
            DropIndex("dbo.ContainerTrackings", new[] { "SecuritySupervisorId" });
            DropIndex("dbo.ContainerTrackings", new[] { "DriverId" });
            AlterColumn("dbo.ContainerTrackings", "SecuritySupervisorId", c => c.Int(nullable: false));
            AlterColumn("dbo.ContainerTrackings", "DriverId", c => c.Int(nullable: false));
            CreateIndex("dbo.ContainerTrackings", "SecuritySupervisorId");
            CreateIndex("dbo.ContainerTrackings", "DriverId");
            AddForeignKey("dbo.ContainerTrackings", "SecuritySupervisorId", "dbo.SecuritySupervisors", "SecuritySupervisorId", cascadeDelete: true);
            AddForeignKey("dbo.ContainerTrackings", "DriverId", "dbo.Drivers", "DriverId", cascadeDelete: true);
        }
    }
}
