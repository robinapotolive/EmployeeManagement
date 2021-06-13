using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleViewModel createRoleViewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(createRoleViewModel.RoleName))
                {
                    IdentityRole identityRole = new IdentityRole { Name = createRoleViewModel.RoleName };
                    var result = await roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        return Ok(identityRole);
                    }
                    if (result.Errors.Any())
                    {
                        return BadRequest(result.Errors);
                    }
                }
                return BadRequest($"Provide role details: {createRoleViewModel.RoleName}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {
            var roles = roleManager.Roles;
            return Ok(roles);
        }

        [HttpGet]
        [Route("GetRoleByRoleId")]
        public async Task<IActionResult> EditRole(string id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    var editRoleViewModel = new EditRoleViewModel
                    {
                        Id = id,
                        RoleName = role.Name
                    };
                    var users = userManager.Users.ToList();

                    foreach (var user in users)
                    {
                        if (await userManager.IsInRoleAsync(user, role.Name))
                        {
                            editRoleViewModel.Users.Add(user.UserName);
                        }
                    }
                    return Ok(editRoleViewModel);
                }
                else
                {
                    return NotFound($"Role Id : {id} not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole([FromBody]EditRoleViewModel editRoleViewModel)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(editRoleViewModel.Id);
                if (role == null)
                {
                    return NotFound($"Role Id : {editRoleViewModel.Id} not found");
                }
                else
                {
                    role.Name = editRoleViewModel.RoleName;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return Ok(role.Name);
                    }
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet]
        [Route("UsersInRole")]
        public async Task<IActionResult> GetUsersInRole(string roleId)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(roleId);
                var userRoleViewModels = new List<UserRoleViewModel>();

                foreach (var user in userManager.Users.ToList())
                {
                    var userViewModel = new UserRoleViewModel()
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userViewModel.IsSelected = true;
                    }
                    else
                    {
                        userViewModel.IsSelected = false;
                    }
                    userRoleViewModels.Add(userViewModel);
                }
                return Ok(userRoleViewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("AddRemoveUserInRole")]
        public async Task<IActionResult> AddRemoveUserInRole([FromBody] List<UserRoleViewModel> userRoleViewModels, string roleId)
        {
            try
            {
                int counter = 0;
                var role = await roleManager.FindByIdAsync(roleId);
                if(role == null)
                {
                    return NotFound("Role id : {roleId} not found");
                }
                foreach (var userRole in userRoleViewModels)
                {
                    counter++;
                    IdentityResult identityResult = null;
                    var user = await userManager.FindByIdAsync(userRole.UserId);
                    if (userRole.IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        identityResult = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!userRole.IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        identityResult = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }                   
                    else
                    {
                        continue;
                    }
                }
                if(userRoleViewModels.Count == counter)
                {
                    return Ok(userRoleViewModels);
                }
                else
                {
                    return BadRequest("Incorrect data");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(Id);
                if(user == null)
                {
                    return NotFound($"User Id: {Id} not found");
                }
                var userClaims = await userManager.GetClaimsAsync(user);
                var userRoles = await userManager.GetRolesAsync(user);
                EditUserViewModel editUserViewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Claims = userClaims.Select(c => c.Value).ToList(),
                    Roles = userRoles
                };
                return Ok(editUserViewModel);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            try
            {
                var user = await userManager.FindByIdAsync(editUserViewModel.Id);
                if(user == null)
                {
                    return NotFound($"User Id: {editUserViewModel.Id} not found");
                }
                user.Email = editUserViewModel.Email;
                user.UserName = editUserViewModel.UserName;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok($"{editUserViewModel}");
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);
                if(user == null)
                {
                    return NotFound($"User Id : {id} not found");
                }
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok($"User id: {id} deleted successfully");
                }
                else
                {
                    return BadRequest($"Unable to delete user id : {id}");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
