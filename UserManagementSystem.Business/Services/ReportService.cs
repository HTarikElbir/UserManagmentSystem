using System.Diagnostics;
using AutoMapper;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.Business.Services;

public class ReportService: IReportService
{
    private readonly IUserService _userService;
    private readonly IDepartmentService _departmentService;
    private readonly IRoleService _roleService;
    private readonly ILogger<ReportService> _logger;
  

    public ReportService(IUserService userService, IMapper mapper, IDepartmentService departmentService, IRoleService roleService, ILogger<ReportService> logger)
    {
        _userService = userService;
        _departmentService = departmentService;
        _roleService = roleService;
        _logger = logger;
    }
    
    public async Task<byte[]> GenerateAllUsersReportAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            _logger.LogInformation("Starting all users report generation");
            
            var users = await _userService.GetAllUsersForReportAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Header().Element(container => ComposeHeader(container, "All Users Report"));
                    page.Content().Element(container => ComposeContent(container, users));
                    page.Footer().Element(ComposeFooter);
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));
                });
            });

            var pdfBytes = document.GeneratePdf();
            
            stopwatch.Stop();
            _logger.LogInformation("All users report generated successfully - Size: {Size}KB, Duration: {Duration}ms, Users: {Count}", 
                pdfBytes.Length / 1024, stopwatch.ElapsedMilliseconds, users.Count);
            
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate all users report after {Duration}ms", stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<byte[]> GenerateDepartmentUsersReportAsync(int departmentId)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            _logger.LogInformation("Starting department users report generation - DepartmentId: {DepartmentId}", departmentId);
            
            var users = await _userService.GetUsersByDepartmentForReportAsync(departmentId);
            var department = await _departmentService.GetByIdAsync(departmentId);
            
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Header().Element(container => ComposeHeader(container, $"{department.DepartmentName} Department Users Report"));
                    page.Content().Element(container => ComposeContent(container, users));
                    page.Footer().Element(ComposeFooter);
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));
                });
            });

            var pdfBytes = document.GeneratePdf();
            
            stopwatch.Stop();
            _logger.LogInformation("Department users report generated successfully - Department: {DepartmentName}, Size: {Size}KB, Duration: {Duration}ms, Users: {Count}", 
                department.DepartmentName, pdfBytes.Length / 1024, stopwatch.ElapsedMilliseconds, users.Count);
            
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate department users report - DepartmentId: {DepartmentId}, Duration: {Duration}ms", 
                departmentId, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<byte[]> GenerateRoleBasedUsersReportAsync(int roleId)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            _logger.LogInformation("Starting role-based users report generation - RoleId: {RoleId}", roleId);
            
            var users = await _userService.GetUsersByRoleForReportAsync(roleId);
            var role = await _roleService.GetRoleByIdAsync(roleId);
            
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Header().Element(container => ComposeHeader(container, $"{role!.RoleName} Role Users Report"));
                    page.Content().Element(container => ComposeContent(container, users));
                    page.Footer().Element(ComposeFooter);
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));
                });
            });
            
            var pdfBytes = document.GeneratePdf();
            
            stopwatch.Stop();
            _logger.LogInformation("Role-based users report generated successfully - Role: {RoleName}, Size: {Size}KB, Duration: {Duration}ms, Users: {Count}", 
                role.RoleName, pdfBytes.Length / 1024, stopwatch.ElapsedMilliseconds, users.Count);
            
            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate role-based users report - RoleId: {RoleId}, Duration: {Duration}ms", 
                roleId, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    // TODO: Add method
    public Task<byte[]> GenerateUsersByStatusReportAsync(bool isActive)
    {
        throw new NotImplementedException();
    }
    
    private void ComposeHeader(IContainer container, string title)
    {
        container.Column(column =>
        {
            column.Item().Text(title)
                .FontSize(20)
                .SemiBold()
                .AlignCenter();

            column.Item().Text($"Generated on: {DateTime.Now:g}")
                .FontSize(10)
                .AlignCenter();
            
            column.Item()
                .PaddingVertical(10)
                .LineHorizontal(1)
                .LineColor(Colors.Grey.Darken2);
        });
    }

    private void ComposeContent(IContainer container, List<UserDto> users)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(1);  // ID
                columns.RelativeColumn(2);  // Username
                columns.RelativeColumn(3);  // Email
                columns.RelativeColumn(2);  // Department
                columns.RelativeColumn(2);  // Roles
            });
            
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("ID").SemiBold();
                header.Cell().Element(CellStyle).Text("Username").SemiBold();;
                header.Cell().Element(CellStyle).Text("Email").SemiBold();
                header.Cell().Element(CellStyle).Text("Department").SemiBold();;
                header.Cell().Element(CellStyle).Text("Roles").SemiBold();;
            });
            
            foreach (var user in users)
            {
                table.Cell().Element(CellStyle).Text(user.UserId.ToString());
                table.Cell().Element(CellStyle).Text(user.Username);
                table.Cell().Element(CellStyle).Text(user.Email);
                table.Cell().Element(CellStyle).Text(user.Department);
                table.Cell().Element(CellStyle).Text(string.Join(", ", user.Roles.Select(r => r.RoleName)));
            }
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().AlignCenter().Text(text =>
            {
                text.Span("Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        });
    }
    
    private IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten1)
            .PaddingVertical(5)
            .PaddingHorizontal(3)
            .AlignMiddle();
    }
}