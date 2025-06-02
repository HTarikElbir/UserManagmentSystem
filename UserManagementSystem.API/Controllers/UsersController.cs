using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;
namespace UserManagementSystem.API.Controllers
{
    //[Authorize]
    [Route("api/users")] 
    [ApiController] 
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService; // Service layer to handle business logic

        // Constructor to inject the IUserService dependency
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        
        // Endpoint to get all users
        [Authorize(Roles = "Admin")]
        [HttpGet] 
        public async Task<IActionResult> GetAllUsersAsync(int page = 1, int pageSize = 10)
        {
            return Ok(await _userService.GetAllUsersAsync(page, pageSize));
        }
        
        [Authorize(Roles = "User")]
        // Endpoint to get a user by their ID
        [HttpGet("by-id/{userId}")] 
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID."); 
            }
            
            var result = await _userService.GetUserByIdAsync(userId);
            
            if (result == null)
            {
                return NotFound("User not found.");
            }
            
            return Ok(result);
        }
        
        [Authorize(Roles = "Admin,User")]
        [HttpPut("{userId:int}")]
        // Updates a user by their ID with the provided updated data
        public async Task<IActionResult> UpdateUserAsync(int userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            bool updateSuccess = await _userService.UpdateUserAsync(userId, userUpdateDto);
            
            if (updateSuccess)
            {
                return NoContent();
            }
            return NotFound();
        }
        
        // Endpoint to delete a user by ID
        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId:int}")] 
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            
            var deleteSuccess = await _userService.DeleteUserAsync(userId);
            
            if (deleteSuccess)
            {
                return NoContent(); 
            }
            
            return NotFound(); 
        }

        // Endpoint to retrieve users based on the specified department name.
        [Authorize(Roles = "Admin")]
        [HttpGet("by-department/{departmentId:int}")]
        public async Task<IActionResult> GetUsersByDepartmentAsync(int departmentId, int page = 1, int pageSize = 10)
        {
            if (departmentId <= 0)
            {
                return BadRequest("Invalid department ID.");
            }
            
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }
            
            var users = await _userService.GetUsersByDepartmentAsync(departmentId, page, pageSize);
            
            return Ok(users);
        }

        // Endpoint to get users by their role
        [Authorize(Roles = "Admin")]
        [HttpGet("by-role/{roleName}")]
        public async Task<IActionResult> GetUsersByRoleAsync(string? roleName, int page = 1, int pageSize = 10)
        {
            if (roleName == null)
            {
                return BadRequest("Invalid role name.");
            }
            
            var users = await _userService.GetUsersByRoleAsync(roleName, page, pageSize);
            
            return Ok(users);
        }
        
        // Endpoint to add a new user
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync(UserAddDto userAddDto)
        {
            var result = await _userService.AddUserAsync(userAddDto);
            
            if (result)
            {
                return Created();
            }
            else
            {
                return BadRequest("User could not be added.");
            }
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUserAsync(AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _userService.AssignRoleToUserAsync(assignRoleDto);
            
            if (result)
            {
                return Ok("Role successfully added to user");
            }
            
            return BadRequest("Role could not be assigned to user.");
        }
    }
}