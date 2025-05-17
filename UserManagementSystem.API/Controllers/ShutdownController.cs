using Microsoft.AspNetCore.Mvc;

namespace UserManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShutdownController : ControllerBase
{
    private readonly IHostApplicationLifetime _applicationLifetime;

    public ShutdownController(IHostApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime;
    }

    [HttpPost("shutdown")]
    public IActionResult Shutdown()
    {
        // Uygulamayı güvenli bir şekilde kapat
        _applicationLifetime.StopApplication();
        return Ok("Uygulama kapatılıyor...");
    }
} 