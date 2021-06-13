using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFormController : ControllerBase
    {
        private IUserFormRepository userFormRepository { get; }
        private readonly ILogger<UserFormController> logger;
        public UserFormController(IUserFormRepository _userFormRepository, ILogger<UserFormController> logger)
        {
            userFormRepository = _userFormRepository;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                var userData = await userFormRepository.Add(user);
                return StatusCode(201, userData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                               
            }
        }
    }
}
