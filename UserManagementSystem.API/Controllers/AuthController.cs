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
            _logger.LogInformation("Login attempt by user: {Username} from IP: {IpAddress}", 
                loginDto.UserName, HttpContext.Connection.RemoteIpAddress);
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for login: {Username}", loginDto.UserName);
                return BadRequest(ModelState);
            }
            
            var token = await _authService.LoginAsync(loginDto);
                
            return Ok(new { Token = token });
        }
        
        // Endpoint for user password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] RequestResetPasswordDto requestDto)
        {
            _logger.LogInformation("Password reset request by email: {Email} from IP: {IpAddress}", 
                requestDto.Email, HttpContext.Connection.RemoteIpAddress);
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for password reset request: {Email}", requestDto.Email);
                return BadRequest(ModelState);
            }
            
            await _authService.RequestResetPasswordAsync(requestDto);
                
            return Ok("Password reset email sent");
           
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
        {
            _logger.LogInformation("Password reset attempt for token: {Token} from IP: {IpAddress}", 
                resetPasswordDto.Token?.Substring(0, 8) + "...", HttpContext.Connection.RemoteIpAddress);
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for password reset from IP: {IpAddress}", 
                    HttpContext.Connection.RemoteIpAddress);
                return BadRequest(ModelState);
            }
            
            await _authService.ResetPasswordAsync(resetPasswordDto);
                
            return Ok("Password reset successful");
        }
       
        [HttpDelete("logout")]
        public async Task<IActionResult> LogoutAsync(LogoutDto logoutDto)
        { 
            _logger.LogInformation("Logout request by user: {Email} from IP: {IpAddress}", 
                logoutDto.Email, HttpContext.Connection.RemoteIpAddress);
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for logout by user: {Email}", logoutDto.Email);
                return BadRequest(ModelState);
            }
            
            await _authService.LogoutAsync(logoutDto);
            return Ok("Logout successful");
        }
    }
}
