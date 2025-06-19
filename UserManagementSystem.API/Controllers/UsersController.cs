using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers
{
    [Authorize]
    [Route("api/users")] 
    [ApiController] 
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService; 
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet] 
        public async Task<IActionResult> GetAllUsersAsync(int page = 1, int pageSize = 10)
        {
            var users = await _userService.GetAllUsersAsync(page, pageSize);
            return Ok(users);
        }
        
        [Authorize(Roles = "Admin,User")]
        [HttpGet("by-id/{userId}")] 
        public async Task<IActionResult> GetUserByIdAsync(int userId) 
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                return BadRequest("Invalid user ID."); 
            }
    
            if (!IsAuthorized(userId))
            {
                _logger.LogWarning("Unauthorized access attempt: UserId={UserId}", userId);
                return Forbid();
            }
            
            var result = await _userService.GetUserByIdAsync(userId); 
            return Ok(result); 
        }
        
        [Authorize(Roles = "Admin,User")]
        [HttpPut("{userId:int}")]
        public async Task<IActionResult> UpdateUserAsync(int userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }
            
            if (!IsAuthorized(userId))
            {
                _logger.LogWarning("Unauthorized update attempt: UserId={UserId}", userId);
                return Forbid();
            }
            
            try
            {
                bool updateSuccess = await _userService.UpdateUserAsync(userId, userUpdateDto);
                
                if (updateSuccess)
                {
                    _logger.LogInformation("UpdateUser successful: UserId={UserId}", userId);
                    return NoContent();
                }
                
                _logger.LogWarning("UpdateUser failed - User not found: UserId={UserId}", userId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateUser failed: UserId={UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId:int}")] 
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID for deletion: {UserId}", userId);
                return BadRequest("Invalid user ID.");
            }
            
            try
            {
                var deleteSuccess = await _userService.DeleteUserAsync(userId);
        
                if (deleteSuccess)
                {
                    _logger.LogInformation("User deleted successfully: UserId={UserId}", userId);
                    return NoContent(); 
                }
        
                _logger.LogWarning("User deletion failed - User not found: UserId={UserId}", userId);
                return NotFound(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteUser failed: UserId={UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("by-department/{departmentId:int}")]
        public async Task<IActionResult> GetUsersByDepartmentAsync(int departmentId, int page = 1, int pageSize = 10)
        {
            if (departmentId <= 0)
            {
                _logger.LogWarning("Invalid department ID: {DepartmentId}", departmentId);
                return BadRequest("Invalid department ID.");
            }
    
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid pagination parameters: Page={Page}, PageSize={PageSize}", page, pageSize); 
                return BadRequest("Page and pageSize must be greater than 0.");
            }
            
            var users = await _userService.GetUsersByDepartmentAsync(departmentId, page, pageSize);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("by-role/{roleName}")]
        public async Task<IActionResult> GetUsersByRoleAsync(string? roleName, int page = 1, int pageSize = 10)
        {
            
            if (roleName == null)
            {
                _logger.LogWarning("Invalid role name: {RoleName}", roleName); 
                return BadRequest("Invalid role name.");
            }
            
            var users = await _userService.GetUsersByRoleAsync(roleName, page, pageSize);
            return Ok(users);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync(UserAddDto userAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
    
            try
            {
                var result = await _userService.AddUserAsync(userAddDto);
        
                if (result)
                {
                    _logger.LogInformation("User created successfully: Email={Email}", userAddDto.Email); 
                    return Created();
                }
                else
                {
                    _logger.LogWarning("User creation failed: Email={Email}", userAddDto.Email);
                    return BadRequest("User could not be added.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddUser failed: Email={Email}", userAddDto.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUserAsync(AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
    
            try
            {
                var result = await _userService.AssignRoleToUserAsync(assignRoleDto);
        
                if (result)
                {
                    _logger.LogInformation("Role assigned successfully: UserId={UserId}, RoleId={RoleId}", assignRoleDto.UserId, assignRoleDto.RoleId);
                    return Ok("Role successfully added to user");
                }
        
                _logger.LogWarning("Role assignment failed: UserId={UserId}, RoleId={RoleId}", assignRoleDto.UserId, assignRoleDto.RoleId);
                return BadRequest("Role could not be assigned to user.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AssignRoleToUser failed: UserId={UserId}, RoleId={RoleId}", assignRoleDto.UserId, assignRoleDto.RoleId);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveRoleFromUserAsync(RemoveRoleDto removeRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
    
            try
            {
                var result = await _userService.RemoveRoleFromUserAsync(removeRoleDto);
        
                if (result)
                {
                    _logger.LogInformation("Role removed successfully: UserId={UserId}, RoleId={RoleId}", removeRoleDto.UserId, removeRoleDto.RoleId);
                    return Ok("Role successfully removed from user");
                }
        
                _logger.LogWarning("Role removal failed: UserId={UserId}, RoleId={RoleId}", removeRoleDto.UserId, removeRoleDto.RoleId);
                return BadRequest("Role could not be removed from user.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RemoveRoleFromUser failed: UserId={UserId}, RoleId={RoleId}", removeRoleDto.UserId, removeRoleDto.RoleId);
                return StatusCode(500, "Internal server error");
            }
        }
        
        private bool IsAuthorized(int userId)
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(nameIdentifier, out var currentUserId))
            {
                return false;
            }
            var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value);
            return userRoles.Contains("Admin") || currentUserId == userId;
        }
    }
}