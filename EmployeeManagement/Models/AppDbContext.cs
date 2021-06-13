using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    //public class AppDbContext : DbContext
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions): base(dbContextOptions)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> UserForm { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Employee>().HasData(
            //    new Employee { Id=2, Name="Mitali", Age=33},
            //    new Employee { Id = 3, Name = "Akshika", Age = 13 }
            //);

            // call the created extension method to seed model data.
            modelBuilder.SeedModelData();
        }
    }
}
