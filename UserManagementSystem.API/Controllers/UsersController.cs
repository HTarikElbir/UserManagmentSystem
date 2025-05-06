using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;
namespace UserManagementSystem.API.Controllers
{
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
        [HttpGet] 
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

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
            else
                return NotFound();
        }

        // Endpoint to delete a user by ID
        [HttpDelete("{userId:int}")] 
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            
            bool deleteSuccess = await _userService.DeleteUserAsync(userId);
            
            if (deleteSuccess)
            {
                return NoContent(); 
            }
            else
                return NotFound(); 
        }

        // Endpoint to retrieve users based on the specified department name.
        [HttpGet("by-department/{departmentName}")]
        public async Task<IActionResult> GetUsersByDepartmentAsync(string? departmentName)
        {
            if (departmentName == null)
            {
                return BadRequest("Invalid department name.");
            }
            
            var users = await _userService.GetUsersByDepartmentAsync(departmentName);
            
            return Ok(users);
        }

        // Endpoint to get users by their role
        [HttpGet("by-role/{roleName}")]
        public async Task<IActionResult> GetUsersByRoleAsync(string? roleName)
        {
            if (roleName == null)
            {
                return BadRequest("Invalid role name.");
            }
            
            var users = await _userService.GetUsersByRoleAsync(roleName);
            
            return Ok(users);
        }
        
        // Endpoint to add new user
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
    }
}