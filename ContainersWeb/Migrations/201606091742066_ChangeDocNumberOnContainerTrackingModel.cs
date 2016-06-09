namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDocNumberOnContainerTrackingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "DocNumber", c => c.String());
            DropColumn("dbo.ContainerTrackings", "DuaNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContainerTrackings", "DuaNumber", c => c.String());
            DropColumn("dbo.ContainerTrackings", "DocNumber");
        }
    }
}
