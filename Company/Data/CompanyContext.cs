using Company.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Data
{
    public class CompanyContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var options = optionsBuilder
                            .UseSqlite(connectionString)
                            .Options;
        }
    }
}
