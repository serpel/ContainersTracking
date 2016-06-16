namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertedByAndUpdatedByFieldsOnContainerTrackingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "InsertedBy", c => c.String());
            AddColumn("dbo.ContainerTrackings", "UpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContainerTrackings", "UpdatedBy");
            DropColumn("dbo.ContainerTrackings", "InsertedBy");
        }
    }
}
