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
        
        // Endpoint for user login
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
        
        // Endpoint for user password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] RequestResetPasswordDto request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var token = await _authService.RequestResetPasswordAsync(request);
            
            if(String.IsNullOrEmpty(token))
                return BadRequest("Invalid token.");
            
            return Ok( new {Token = token});
        }
    }
}
