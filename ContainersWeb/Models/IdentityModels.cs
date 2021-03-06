﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ContainersWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
     
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<ContainerTracking> ContainerTracking { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<TrailerTracking> TrailerTracking { get; set; }
        public DbSet<TruckTracking> TruckTracking { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<SecuritySupervisor> SecuritySupervisors { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

    }
}