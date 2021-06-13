using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Route("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var user = new IdentityUser { UserName = registerViewModel.Email, Email = registerViewModel.Email };
            var result = await userManager.CreateAsync(user, registerViewModel.Password);
            try
            {               
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(user);
                }
                if (result.Errors.Any())
                {
                    return BadRequest(result.Errors);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(result.Errors);
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                    loginViewModel.RememberMe, false);

                if (result.Succeeded)
                {
                    return Ok($"Welcome : {loginViewModel.Email}");
                }
                else
                {
                    return NotFound("Invalid Credentails");
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GoogleLogin")]
        public async Task<IActionResult> Login(string returunUrl)
        {
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returunUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return Ok(loginViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ExternalLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }


        [HttpGet]
        [Route("ExternalLoginCallback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl=null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if(remoteError != null)
            {
                return BadRequest($"Error from external provider: {remoteError}");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                return BadRequest($"Error loading external login information");
            }
            return Ok();
        }
    }
}
