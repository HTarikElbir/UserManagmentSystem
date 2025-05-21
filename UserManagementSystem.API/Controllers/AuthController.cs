using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Dtos.Auth;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            
            if(result)
                return Ok();
            else
                return BadRequest();
        }
        [AllowAnonymous]
        [HttpDelete("logout")]
        public async Task<IActionResult> LogoutAsync(LogoutDto logoutDto)
        { 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            //token = token.Replace("Bearer ", "");

            var result = await _authService.LogoutAsync(logoutDto);
    
            if (result)
            {
                return Ok(new { message = "Successfully logged out" });
            }
    
            return BadRequest("Logout failed");
        }
    }
}
