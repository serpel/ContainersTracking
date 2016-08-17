namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedFieldOnContainerTrackingModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContainerTrackings", "CompanyDestinationId", "dbo.Companies");
            DropForeignKey("dbo.ContainerTrackings", "CompanyOriginId", "dbo.Companies");
            DropIndex("dbo.ContainerTrackings", new[] { "CompanyOriginId" });
            DropIndex("dbo.ContainerTrackings", new[] { "CompanyDestinationId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.ContainerTrackings", "CompanyDestinationId");
            CreateIndex("dbo.ContainerTrackings", "CompanyOriginId");
            AddForeignKey("dbo.ContainerTrackings", "CompanyOriginId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.ContainerTrackings", "CompanyDestinationId", "dbo.Companies", "CompanyId");
        }
    }
}
