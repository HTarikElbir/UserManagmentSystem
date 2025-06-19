using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsersReportAsync()
        {
            try
            {
                var reportBytes = await _reportService.GenerateAllUsersReportAsync();
                _logger.LogInformation("All users report generated successfully: ReportSize={ReportSize} bytes", reportBytes.Length);
                return File(
                    reportBytes,
                    "application/pdf",
                    $"all-users-report-{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllUsersReport failed");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-department/{departmentId:int}")]
        public async Task<IActionResult> GetUsersByDepartmentReportAsync(int departmentId)
        {
            if (departmentId <= 0)
            {
                return BadRequest("Invalid department ID.");
            }

            try
            {
                var reportBytes = await _reportService.GenerateDepartmentUsersReportAsync(departmentId);
                _logger.LogInformation("Department users report generated successfully: DepartmentId={DepartmentId}, ReportSize={ReportSize} bytes", departmentId, reportBytes.Length);
                return File(
                    reportBytes,
                    "application/pdf",
                    $"department-users-report-{departmentId}-{DateTime.Now:yyyyMMdd}.pdf"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUsersByDepartmentReport failed: DepartmentId={DepartmentId}", departmentId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-role/{roleId:int}")]
        public async Task<IActionResult> GetUsersByRoleReportAsync(int roleId)
        {
            if (roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }
            
            try
            {
                var reportBytes = await _reportService.GenerateRoleBasedUsersReportAsync(roleId);
                _logger.LogInformation("Role users report generated successfully: RoleId={RoleId}, ReportSize={ReportSize} bytes", roleId, reportBytes.Length);
                
                return File(
                    reportBytes,
                    "application/pdf",
                    $"role-users-report-{roleId}-{DateTime.Now:yyyyMMdd}.pdf"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUsersByRoleReport failed: RoleId={RoleId}", roleId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
