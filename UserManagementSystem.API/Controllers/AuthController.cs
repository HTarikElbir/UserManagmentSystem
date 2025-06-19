using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        
        // Endpoint for user login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Login request received: {Username}", loginDto.UserName);
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                _logger.LogInformation("Login successful: {Username}", loginDto.UserName);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed: {Username}", loginDto.UserName);
                return Unauthorized($"{ex.Message}");
            }
        }
        
        // Endpoint for user password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] RequestResetPasswordDto request)
        {
            _logger.LogInformation("Forgot password request received: {Email}", request.Email);
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var token = await _authService.RequestResetPasswordAsync(request);
                
                if(String.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Invalid token generated for forgot password: {Email}", request.Email);
                    return BadRequest("Invalid token.");
                }
                
                _logger.LogInformation("Forgot password token generated: {Email}", request.Email);
                return Ok(new {Token = token});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Forgot password request failed: {Email}", request.Email);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
        {
            _logger.LogInformation("Reset password request received: {Email}", resetPasswordDto.Email);
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var result = await _authService.ResetPasswordAsync(resetPasswordDto);
                
                if(result)
                {
                    _logger.LogInformation("Password reset successful: {Email}", resetPasswordDto.Email);
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Password reset failed: {Email}", resetPasswordDto.Email);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reset password failed: {Email}", resetPasswordDto.Email);
                return BadRequest(ex.Message);
            }
        }
       
        [HttpDelete("logout")]
        public async Task<IActionResult> LogoutAsync(LogoutDto logoutDto)
        { 
            _logger.LogInformation("Logout request received: {Email}", logoutDto.Email);
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _authService.LogoutAsync(logoutDto);
        
                if (result)
                {
                    _logger.LogInformation("Logout successful: {Email}", logoutDto.Email);
                    return Ok(new { message = "Successfully logged out" });
                }
        
                _logger.LogWarning("Logout failed: {Email}", logoutDto.Email);
                return BadRequest("Logout failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed: {Email}", logoutDto.Email);
                return BadRequest(ex.Message);
            }
        }
    }
}
