using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos;
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
                return BadRequest(ModelState);
            }
            
            try
            {
                var result = await _roleService.AddRoleAsync(roleAddDto);
                
                if (result)
                {
                    _logger.LogInformation("Role created successfully: RoleName={RoleName}", roleAddDto.RoleName); 
                    return Created();
                }
                else
                {
                    _logger.LogWarning("Role creation failed: RoleName={RoleName}", roleAddDto.RoleName);
                    return BadRequest("Role could not be added.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddRole failed: RoleName={RoleName}", roleAddDto.RoleName);
                return StatusCode(500, "Internal server error");
            }
        }
        
        // Endpoint to delete a role by ID
        [HttpDelete("{roleId:int}")]
        public async Task<IActionResult> DeleteRoleAsync(int roleId)
        {
            if (roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }

            try
            {
                var result = await _roleService.DeleteRoleAsync(roleId);

                if (result)
                {
                    _logger.LogInformation("Role deleted successfully: RoleId={RoleId}", roleId); 
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning("Role deletion failed - Role not found: RoleId={RoleId}", roleId);
                    return NotFound("Role not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteRole failed: RoleId={RoleId}", roleId);
                return StatusCode(500, "Internal server error");
            }
            
        }
        
        // Endpoint to update a role by ID
        [HttpPut("{roleId:int}")]
        public async Task<IActionResult> UpdateRoleAsync(int roleId, [FromBody] RoleUpdateDto roleUpdateDto)
        {
            if (roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var result = await _roleService.UpdateRoleAsync(roleId, roleUpdateDto);

                if (result)
                {
                    _logger.LogInformation("Role updated successfully: RoleId={RoleId}, NewRoleName={NewRoleName}", roleId, roleUpdateDto.RoleName);
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning("Role update failed - Role not found: RoleId={RoleId}", roleId);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateRole failed: RoleId={RoleId}", roleId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
