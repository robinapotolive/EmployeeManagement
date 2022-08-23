using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeePayrollReoisitory : IEmployeePayrollRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmployeePayrollReoisitory(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Payroll> AddEmployeeSalaryAsync(Payroll payroll)
        {
            await _appDbContext.Payrolls.AddAsync(payroll);
            await _appDbContext.SaveChangesAsync();
            return payroll;
        }

        public async Task<decimal> GetNthHighstSalary(int whichHighstSalary)
        {
            decimal nthHighstSalay = 0.0M;
            var salaryList = await _appDbContext.Payrolls.ToListAsync();
            if (salaryList.Any())
            {
                var salary = salaryList.OrderByDescending(x => x.Salary)
                            .Select(x => x.Salary).Distinct()
                            .Take(whichHighstSalary)
                            .Skip(whichHighstSalary - 1).FirstOrDefault();
                if (salary.HasValue)
                {
                    nthHighstSalay = salary.Value;
                }
            }
            return nthHighstSalay;
        }
    }
}
