namespace ContainersWeb.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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
            createUser(context);

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

        public void createUser(ApplicationDbContext context)
        {
            string email = "sergio.peralta@kattangroup.com";

            //create the first user
            if (!(context.Users.Any(u => u.Email == email)))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = email, Email = email };
                var result = userManager.Create(userToInsert, "Admin.1234");

                //create and asign roles
                if (result.Succeeded)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var adminRole = new IdentityRole("Admin");
                    result = roleManager.Create(adminRole);

                    if (result.Succeeded)
                    {
                        userManager.AddToRoles(userToInsert.Id, adminRole.Name);
                    }
                }
            }
        }
    }
}
