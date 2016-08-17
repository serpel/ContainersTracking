namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOriginAndDestionationModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        DestinationId = c.Int(nullable: false, identity: true),
                        ContainerTrackingId = c.Int(nullable: false),
                        CompanyDestinationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DestinationId)
                .ForeignKey("dbo.Companies", t => t.CompanyDestinationId, cascadeDelete: true)
                .ForeignKey("dbo.ContainerTrackings", t => t.ContainerTrackingId, cascadeDelete: true)
                .Index(t => t.ContainerTrackingId)
                .Index(t => t.CompanyDestinationId);
            
            CreateTable(
                "dbo.Origins",
                c => new
                    {
                        OriginId = c.Int(nullable: false, identity: true),
                        ContainerTrackingId = c.Int(nullable: false),
                        CompanyOriginId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OriginId)
                .ForeignKey("dbo.Companies", t => t.CompanyOriginId, cascadeDelete: true)
                .ForeignKey("dbo.ContainerTrackings", t => t.ContainerTrackingId, cascadeDelete: true)
                .Index(t => t.ContainerTrackingId)
                .Index(t => t.CompanyOriginId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Origins", "ContainerTrackingId", "dbo.ContainerTrackings");
            DropForeignKey("dbo.Origins", "CompanyOriginId", "dbo.Companies");
            DropForeignKey("dbo.Destinations", "ContainerTrackingId", "dbo.ContainerTrackings");
            DropForeignKey("dbo.Destinations", "CompanyDestinationId", "dbo.Companies");
            DropIndex("dbo.Origins", new[] { "CompanyOriginId" });
            DropIndex("dbo.Origins", new[] { "ContainerTrackingId" });
            DropIndex("dbo.Destinations", new[] { "CompanyDestinationId" });
            DropIndex("dbo.Destinations", new[] { "ContainerTrackingId" });
            DropTable("dbo.Origins");
            DropTable("dbo.Destinations");
        }
    }
}
