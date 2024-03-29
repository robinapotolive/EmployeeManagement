﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext appDbContext;

        public SqlEmployeeRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<Employee> Add(Employee employee)
        {
           await appDbContext.AddAsync(employee);
           await appDbContext.SaveChangesAsync();
           return employee;
        }

        public async Task<Employee> Delete(int id)
        {
            var emp = await appDbContext.Employees.FindAsync(id);
            if(emp != null)
            {
                appDbContext.Employees.Remove(emp);
                await appDbContext.SaveChangesAsync();
            }
            return emp;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeeByUsingStoredProcedure()
        {
            return await appDbContext.Employees
                    .FromSqlRaw("EXECUTE dbo.GetAllEmployees")
                    .ToListAsync();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await appDbContext.Employees.FindAsync(id);          
        }

        public async Task<IEnumerable<Employee>> GetEmployeeByNameInterpolated(string name)
        {
            return await appDbContext.Employees
                         .FromSqlInterpolated($"EXECUTE dbo.GetAllEmployeeByName {name}")
                         .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeeByNameParameter(string name)
        {
            return await appDbContext.Employees
                    .FromSqlRaw("EXECUTE dbo.GetAllEmployeeByName {0}", name)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeeByRawSql()
        {
            return await appDbContext.Employees
                            .FromSqlRaw("SELECT * FROM dbo.Employees")
                            .OrderBy(o => o.Name)
                            .ToListAsync();            
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await appDbContext.Employees.ToListAsync();
        }

        public async Task<Employee> Update(Employee employee)
        {
            var emp = appDbContext.Employees.Attach(employee);
            emp.State = EntityState.Modified;
            await appDbContext.SaveChangesAsync();
            return employee;
        }
    }
}
