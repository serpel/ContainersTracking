namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObservationsFieldOnContainerTrackingsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "Observations", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContainerTrackings", "Observations");
        }
    }
}
