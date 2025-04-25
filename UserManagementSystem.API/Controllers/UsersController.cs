using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Data.Entities;
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
            // Call the service to get all users and return the result with a 200 OK status
            return Ok(await _userService.GetAllUsersAsync());
        }

        // Endpoint to get a user by their ID
        [HttpGet("{userId}")] 
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            // If the userId is invalid (e.g., less than or equal to 0), return a 400 BadRequest
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID."); 
            }

            // Call the service to get the user by ID
            var result = await _userService.GetUserByIdAsync(userId);
            
            // If the user is not found, return a 404 NotFound
            if (result == null)
            {
                return NotFound("User not found.");
            }

            // If the user is found, return the result with a 200 OK status
            return Ok(result);
        }
        
        
    }
}