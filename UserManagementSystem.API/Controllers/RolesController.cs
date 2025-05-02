using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            // Return the list of roles with a 200-OK status
            return Ok(await _roleService.GetAllRolesAsync());
        }
        
        [HttpGet("by-id/{roleId:int}")]
        public async Task<IActionResult> GetRoleByIdAsync(int roleId)
        {
            // If the roleId is invalid, return a 400-BadRequest
            if (roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }

            // Call the service to get the role by ID
            var result = await _roleService.GetRoleByIdAsync(roleId);
            
            // If the role is not found, return a 404-NotFound
            if (result == null)
            {
                return NotFound("Role not found.");
            }

            // If the role is found, return the result with a 200-OK status
            return Ok(result);
        }
        
        [HttpGet("by-name/{roleName}")]
        public async Task<IActionResult> GetRoleByNameAsync(string roleName)
        {
            // If the roleName is null or empty, return a 400-BadRequest
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be null or empty.");
            }

            // Call the service to get the role by name
            var result = await _roleService.GetRoleByNameAsync(roleName);
            
            // If the role is not found, return a 404-NotFound
            if (result == null)
            {
                return NotFound("Role not found.");
            }

            // If the role is found, return the result with a 200-OK status
            return Ok(result);
        }
    }
}
