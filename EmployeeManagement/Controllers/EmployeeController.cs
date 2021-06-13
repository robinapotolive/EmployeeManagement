using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            this.logger = logger;
        }

        [HttpGet]
        [Route("getAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await this._employeeRepository.GetEmployees();
                return Ok(employees);
            }
            catch (Exception)
            {
                return BadRequest($"Error on hitting the request");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                logger.LogTrace("this is a trace message");
                logger.LogInformation("this is a information message");
                logger.LogWarning("this is warning message");
                logger.LogError("this is error message");
                logger.LogCritical("this is critical message");
                if (id == 0)
                {
                    id = 1;
                }
                string name = string.Empty;
                var emp = await this._employeeRepository.GetEmployee(id);
                if (emp != null)
                {
                    name = emp.Name;
                    logger.LogInformation($"Get Name {emp.Name}");
                    return Ok(name);
                }
                logger.LogInformation($"Employee not found {id}");
                return NotFound($"Employee {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            try
            {
                var emp = await this._employeeRepository.Add(employee);
                return StatusCode(201, emp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Employee employee)
        {
            try
            {
                var emp = await this._employeeRepository.Update(employee);
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var emp = await this._employeeRepository.Delete(id);
                if (emp == null)
                {
                    return NotFound($"Employee {id} not found");
                }
                return Ok(emp);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
    }
}
