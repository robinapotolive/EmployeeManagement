using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtAuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public JwtAuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetJwtToken")]
        public IActionResult Login([FromBody]UserModel userModel)        
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(userModel);
            if(user != null)
            {
                var tokenString = GenerateJsonWebToken(user);
                response =  Ok(new { token = tokenString });
            }
            return response;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // this is important to authenticate
        [HttpGet]
        [Route("GetUsers")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Robin","Mitali","Akshika","Adyak"};
        }
        private string GenerateJsonWebToken(UserModel userModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] { 
                new Claim("UserName","Robin"),
                new Claim("Role","User")
            };
            var token = new JwtSecurityToken(
                configuration["JWT:ValidIssuer"],
                configuration["JWT:ValidAudience"],
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //To authenticate user.
        private UserModel AuthenticateUser(UserModel userModel)
        {
            UserModel user = null;
            if(userModel.UserName == "robin")
            {
                user = new UserModel { UserName = "robin", EmailAddress = "rapoto@gmail.com" };
            }
            return user;
        }
    }
}
