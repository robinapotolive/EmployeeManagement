using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        List<Employee> employees = new List<Employee>()
        {
            new Employee{Id=1, Name="Robin", Age=2},
            new Employee{Id=2, Name="Mitali", Age=5},
            new Employee{ Id=3, Name="Akshika", Age=7},
            new Employee{Id=4, Name="Adyak", Age=10}
        };

        public async Task<Employee> Add(Employee employee)
        {
            employee.Id = employees.Max(x => x.Id) + 1;
            employees.Add(employee);
            return await Task.FromResult(employee);
        }

        public async Task<Employee> Delete(int id)
        {
            var emp = employees.Find(x => x.Id == id);
            if(emp != null)
            {
                employees.Remove(emp);
            }
            return await Task.FromResult(emp);
        }

        public Task<IEnumerable<Employee>> GetAllEmployeeByUsingStoredProcedure()
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await Task.FromResult(employees.Find(x => x.Id == id));            
        }

        public Task<IEnumerable<Employee>> GetEmployeeByNameInterpolated(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> GetEmployeeByRawSql()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await Task.FromResult(employees);
        }

        public async Task<Employee> Update(Employee employee)
        {
            var emp = employees.Find(x => x.Id == employee.Id);
            emp.Id = employee.Id;
            emp.Name = employee.Name;
            emp.Age = emp.Age;
            employees.Add(emp);
            return await Task.FromResult(emp);
        }

        Task<IEnumerable<Employee>> IEmployeeRepository.GetEmployeeByNameParameter(string name)
        {
            throw new NotImplementedException();
        }
    }
}
