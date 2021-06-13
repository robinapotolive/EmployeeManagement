using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployee(int id);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> Add(Employee employee);
        Task<Employee> Update(Employee employee);
        Task<Employee> Delete(int id);

    }
}
