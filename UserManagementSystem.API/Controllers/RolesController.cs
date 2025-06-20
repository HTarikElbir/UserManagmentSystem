using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers
{
    [Authorize(Roles="Admin")]
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        
        // Endpoint to get all roles
        [HttpGet]
        public async Task<IActionResult> GetAllRolesAsync(int page = 1, int pageSize = 10)
        { 
                var roles = await _roleService.GetAllRolesAsync(page, pageSize);
                return Ok(roles);
        }
        
        // Endpoint to get a role by their ID
        [HttpGet("by-id/{roleId:int}")]
        public async Task<IActionResult> GetRoleByIdAsync(int roleId)
        {
            if (roleId <= 0)
            {
                _logger.LogWarning("Invalid role ID: {RoleId}", roleId);
                return BadRequest("Invalid role ID.");
            }
            
            var result = await _roleService.GetRoleByIdAsync(roleId);
        
            if (result == null)
            {
               return NotFound("Role not found.");
            }
            return Ok(result);
        }
        
        // Endpoint to get a role by name
        [HttpGet("by-name/{roleName}")]
        public async Task<IActionResult> GetRoleByNameAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                _logger.LogWarning("Invalid role name: {RoleName}", roleName);
                return BadRequest("Role name cannot be null or empty.");
            }
            
            
            var result = await _roleService.GetRoleByNameAsync(roleName);
                
            if (result == null)
            {
                return NotFound("Role not found.");
            }
                
            return Ok(result);
           
        }
        
        // Endpoint to add a new role
        [HttpPost]
        public async Task<IActionResult> AddRoleAsync(RoleAddDto roleAddDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for add role: {RoleName}", roleAddDto.RoleName);
                return BadRequest(ModelState);
            }

            var result = await _roleService.AddRoleAsync(roleAddDto);

            if (result)
            {
                return Created();
            }
            
            return BadRequest("Role could not be added.");
            
        }
        
        // Endpoint to delete a role by ID
        [HttpDelete("{roleId:int}")]
        public async Task<IActionResult> DeleteRoleAsync(int roleId)
        {
            if (roleId <= 0)
            {
                _logger.LogWarning("Invalid role ID for deletion: {RoleId}", roleId);
                return BadRequest("Invalid role ID.");
            }

            var result = await _roleService.DeleteRoleAsync(roleId);

            if (result)
            {
                return NoContent();
            }
           
            return NotFound("Role not found.");
        }
        
        // Endpoint to update a role by ID
        [HttpPut("{roleId:int}")]
        public async Task<IActionResult> UpdateRoleAsync(int roleId, [FromBody] RoleUpdateDto roleUpdateDto)
        {
            if (roleId <= 0)
            {
                _logger.LogWarning("Invalid role ID for update: {RoleId}", roleId);
                return BadRequest("Invalid role ID.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for update role: {RoleId}", roleId);
                return BadRequest(ModelState);
            }
            
            var result = await _roleService.UpdateRoleAsync(roleId, roleUpdateDto);

            if (result)
            {
                return NoContent();
            }
            
            return NotFound();
        }
    }
}
