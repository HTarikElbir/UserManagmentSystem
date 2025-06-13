using System.Diagnostics;

namespace UserManagementSystem.API.Middlewares;

public class LoggingMiddleware
{
    // private readonly RequestDelegate _next;
    // private readonly ILogger<LoggingMiddleware> _logger;
    //
    // public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    // {
    //     _next = next;
    //     _logger = logger;
    // }
    //
    // public async Task InvokeAsync(HttpContext context)
    // {
    //     var stopwatch = Stopwatch.StartNew();
    //         
    //     // Request bilgilerini logla
    //     _logger.LogInformation(
    //         "Request: {Method} {Path} started at {Time}",
    //         context.Request.Method,
    //         context.Request.Path,
    //         DateTime.UtcNow);
    //
    //     // Response'u bekle
    //     await _next(context);
    //
    //     stopwatch.Stop();
    //
    //     // Response bilgilerini logla
    //     _logger.LogInformation(
    //         "Response: {Method} {Path} completed with status code {StatusCode} in {ElapsedMilliseconds}ms",
    //         context.Request.Method,
    //         context.Request.Path,
    //         context.Response.StatusCode,
    //         stopwatch.ElapsedMilliseconds);
    // }
}