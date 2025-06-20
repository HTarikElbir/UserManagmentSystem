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
            
            var updateSuccess = await _userService.UpdateUserAsync(userId, userUpdateDto);
            
            if (updateSuccess)
            {
                return NoContent();
            }
            
            return NotFound();
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
            
            var deleteSuccess = await _userService.DeleteUserAsync(userId);

            if (deleteSuccess)
            {
                return NoContent(); 
            }

            return NotFound(); 
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
                _logger.LogWarning("Invalid model state for add user: {Email}", userAddDto.Email);
                return BadRequest(ModelState);
            }
    
            var result = await _userService.AddUserAsync(userAddDto);
        
            if (result)
            {
                return Created();
            }
            
            return BadRequest("User could not be added.");
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUserAsync(AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for assign role: UserId={UserId}, RoleId={RoleId}", assignRoleDto.UserId, assignRoleDto.RoleId);
                return BadRequest(ModelState);
            }
    
            var result = await _userService.AssignRoleToUserAsync(assignRoleDto);

            if (result)
            {
                return Ok("Role successfully added to user");
            }

            return BadRequest("Role could not be assigned to user.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveRoleFromUserAsync(RemoveRoleDto removeRoleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for remove role: UserId={UserId}, RoleId={RoleId}", removeRoleDto.UserId, removeRoleDto.RoleId);
                return BadRequest(ModelState);
            }
    
            var result = await _userService.RemoveRoleFromUserAsync(removeRoleDto);

            if (result)
            {
                return Ok("Role successfully removed from user");
            }

            return BadRequest("Role could not be removed from user.");
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