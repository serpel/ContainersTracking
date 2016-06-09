namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrelativeAduanaOnContainerTrackingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "CorrelAduana", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContainerTrackings", "CorrelAduana");
        }
    }
}
