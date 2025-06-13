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

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsersReportAsync()
        {
            var reportBytes = await _reportService.GenerateAllUsersReportAsync();
            return File(
                reportBytes,
                "application/pdf",
                $"all-users-report-{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("by-department/{departmentId:int}")]
        public async Task<IActionResult> GetUsersByDepartmentReportAsync(int departmentId)
        {
            if (departmentId <= 0)
            {
                return BadRequest("Invalid department ID.");
            }

            var reportBytes = await _reportService.GenerateDepartmentUsersReportAsync(departmentId);
            return File(
                reportBytes,
                "application/pdf",
                $"department-users-report-{departmentId}-{DateTime.Now:yyyyMMdd}.pdf"
            );
        }

        [HttpGet("by-role/{roleId:int}")]
        public async Task<IActionResult> GetUsersByRoleReportAsync(int roleId)
        {
            if (roleId <= 0)
            {
                return BadRequest("Invalid role ID.");
            }
            
            var reportBytes = await _reportService.GenerateRoleBasedUsersReportAsync(roleId);
            
            return File(
                reportBytes,
                "application/pdf",
                $"role-users-report-{roleId}-{DateTime.Now:yyyyMMdd}.pdf"
            );
        }
    }
}
