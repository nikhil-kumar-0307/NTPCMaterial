namespace NTPCMaterial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using NTPCMaterial.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<NTPCMaterial.Data.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NTPCMaterial.Data.AppDbContext context)
        {
            context.Employees.AddOrUpdate(
                e => e.EmployeeNumber,
                new Employee
                {
                    EmployeeNumber = "EMP001",
                    PasswordHash = HashPassword("Admin@123"),
                    Role = "Admin",
                    LastLoginAt = null
                },
                new Employee
                {
                    EmployeeNumber = "EMP002",
                    PasswordHash = HashPassword("User@123"),
                    Role = "User",
                    LastLoginAt = null
                }
            );
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}