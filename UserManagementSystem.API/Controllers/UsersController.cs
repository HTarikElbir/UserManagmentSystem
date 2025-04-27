using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos;
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
        public async Task<IActionResult> GetAllUsers()
        {
            // Call the service to get all users and return the result with a 200-OK status
            return Ok(await _userService.GetAllUsersAsync());
        }

        // Endpoint to get a user by their ID
        [HttpGet("{userId}")] 
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            // If the userId is invalid (e.g., less than or equal to 0), return a 400-BadRequest
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID."); 
            }

            // Call the service to get the user by ID
            var result = await _userService.GetUserByIdAsync(userId);
            
            // If the user is not found, return a 404-NotFound
            if (result == null)
            {
                return NotFound("User not found.");
            }

            // If the user is found, return the result with a 200-OK status
            return Ok(result);
        }

        [HttpPut("{userId}")]
        // Updates a user by their ID with the provided updated data
        public async Task<IActionResult> UpdateUserAsync(int userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            // Check if the incoming model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
    
            // Attempt to update the user
            bool updateSuccess = await _userService.UpdateUserAsync(userId, userUpdateDto);
    
            // If update is successful, return 204 No Content
            if (updateSuccess)
            {
                return NoContent();
            }
            else
                // If user not found, return 404 Not Found
                return NotFound();
        }

        [HttpDelete("{userId}")]
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
        
    }
}