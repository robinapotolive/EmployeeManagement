using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePayrollController : ControllerBase
    {
        private readonly IEmployeePayrollRepository _employeePayroll;

        public EmployeePayrollController(IEmployeePayrollRepository employeePayroll)
        {
            _employeePayroll = employeePayroll;
        }
       
        // GET api/<EmployeePayrollController1>/5
        [HttpGet("{id}")]        
        public async Task<IActionResult> GetNthHighstSalary(int id)
        {
            try
            {
                var result = await _employeePayroll.GetNthHighstSalary(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<EmployeePayrollController1>
        [HttpPost]
        [Route("AddEmployeeSalary")]
        public async Task<IActionResult> AddEmployeeSalary([FromBody] Payroll payroll)
        {
            try
            {
                var result = await _employeePayroll.AddEmployeeSalaryAsync(payroll);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // PUT api/<EmployeePayrollController1>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeePayrollController1>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
