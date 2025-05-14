using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.Auth;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized($"{ex.Message}");
            }
        }
    }
}
