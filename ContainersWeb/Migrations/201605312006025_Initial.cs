namespace ContainersWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Code = c.String(),
                        RegionId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.ContainerTrackings",
                c => new
                    {
                        ContainerTrackingId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        CompanyOriginId = c.Int(),
                        CompanyDestinationId = c.Int(),
                        DocStatus = c.Int(nullable: false),
                        ContainerNumber = c.String(nullable: false),
                        ContainerLicensePlate = c.String(),
                        ContainerLabel = c.String(),
                        ChasisNumber = c.String(),
                        DuaNumber = c.String(),
                        DriverId = c.Int(nullable: false),
                        SecuritySupervisorId = c.Int(nullable: false),
                        InsertedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Company_CompanyId = c.Int(),
                        Company_CompanyId1 = c.Int(),
                    })
                .PrimaryKey(t => t.ContainerTrackingId)
                .ForeignKey("dbo.Companies", t => t.CompanyDestinationId)
                .ForeignKey("dbo.Companies", t => t.CompanyOriginId)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.SecuritySupervisors", t => t.SecuritySupervisorId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId)
                .ForeignKey("dbo.Companies", t => t.Company_CompanyId1)
                .Index(t => t.CompanyOriginId)
                .Index(t => t.CompanyDestinationId)
                .Index(t => t.DriverId)
                .Index(t => t.SecuritySupervisorId)
                .Index(t => t.Company_CompanyId)
                .Index(t => t.Company_CompanyId1);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        DriverId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CardId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DriverId);
            
            CreateTable(
                "dbo.SecuritySupervisors",
                c => new
                    {
                        SecuritySupervisorId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CardId = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SecuritySupervisorId);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        RegionId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RegionId);
            
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Username = c.String(),
                        Level = c.String(),
                        Message = c.String(),
                        Exception = c.String(),
                        Logger = c.String(),
                        CallSite = c.String(),
                        ServerName = c.String(),
                        Port = c.String(),
                        Url = c.String(),
                        RemoteAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Companies", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.ContainerTrackings", "Company_CompanyId1", "dbo.Companies");
            DropForeignKey("dbo.ContainerTrackings", "Company_CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ContainerTrackings", "SecuritySupervisorId", "dbo.SecuritySupervisors");
            DropForeignKey("dbo.ContainerTrackings", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.ContainerTrackings", "CompanyOriginId", "dbo.Companies");
            DropForeignKey("dbo.ContainerTrackings", "CompanyDestinationId", "dbo.Companies");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ContainerTrackings", new[] { "Company_CompanyId1" });
            DropIndex("dbo.ContainerTrackings", new[] { "Company_CompanyId" });
            DropIndex("dbo.ContainerTrackings", new[] { "SecuritySupervisorId" });
            DropIndex("dbo.ContainerTrackings", new[] { "DriverId" });
            DropIndex("dbo.ContainerTrackings", new[] { "CompanyDestinationId" });
            DropIndex("dbo.ContainerTrackings", new[] { "CompanyOriginId" });
            DropIndex("dbo.Companies", new[] { "RegionId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.LogEntries");
            DropTable("dbo.Regions");
            DropTable("dbo.SecuritySupervisors");
            DropTable("dbo.Drivers");
            DropTable("dbo.ContainerTrackings");
            DropTable("dbo.Companies");
        }
    }
}
