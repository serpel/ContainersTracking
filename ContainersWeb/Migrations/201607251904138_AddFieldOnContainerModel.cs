namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldOnContainerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContainerTrackings", "DUA", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContainerTrackings", "DUA");
        }
    }
}
