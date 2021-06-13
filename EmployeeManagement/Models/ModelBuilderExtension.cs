using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void SeedModelData(this ModelBuilder modelBuilder)
        {
            // to seed the initial model data.
            modelBuilder.Entity<Employee>().HasData(
               new Employee { Id = 2, Name = "Mitali", Age = 33 },
               new Employee { Id = 3, Name = "Akshika", Age = 13 },
               new Employee { Id = 4, Name = "Adyak", Age = 6 }
           );
        }
    }
}
