namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsActiveFieldOnDriverModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "IsActive");
        }
    }
}
