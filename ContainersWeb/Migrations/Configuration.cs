namespace ContainersWeb.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ContainersWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ContainersWeb.Models.ApplicationDbContext context)
        {
            context.Regions.AddOrUpdate(
                p => p.RegionId,
                new Region { Name = "Inhdelva Norte", RegionId = 1 },
                new Region { Name = "Inhdelva Sur", RegionId = 2 },
                new Region { Name = "Inhdelva Oeste", RegionId = 3 },
                new Region { Name = "Inhdelva Este", RegionId = 4 }
                );

            context.Companies.AddOrUpdate(
                p => p.CompanyId,
                new Company { Name = "Testing", RegionId = 1 }
                );
        }
    }
}
