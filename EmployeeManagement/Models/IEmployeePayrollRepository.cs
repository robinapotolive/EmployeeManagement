using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IEmployeePayrollRepository
    {
        Task<Payroll> AddEmployeeSalaryAsync(Payroll payroll);

        Task<decimal> GetNthHighstSalary(int whichHighstSalary);
    }
}
