namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSecurityModel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SecuritySupervisors", "CardIdIndex");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.SecuritySupervisors", "CardId", unique: true, name: "CardIdIndex");
        }
    }
}
