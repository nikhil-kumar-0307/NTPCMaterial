using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using NTPCMaterial.Models;

namespace NTPCMaterial.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("AppDbContext")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<AppDbContext,
                NTPCMaterial.Migrations.Configuration>()
            );
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<MaterialPurchase> MaterialPurchases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
                        .Configure(c => c.HasColumnType("datetime2"));
            base.OnModelCreating(modelBuilder);
        }
    }
}