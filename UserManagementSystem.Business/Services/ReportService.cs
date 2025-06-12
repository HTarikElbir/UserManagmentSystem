using AutoMapper;
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
  

    public ReportService(IUserService userService, IMapper mapper, IDepartmentService departmentService, IRoleService roleService)
    {
        _userService = userService;
        _departmentService = departmentService;
        _roleService = roleService;
    }
    
    public async Task<byte[]> GenerateAllUsersReportAsync()
    {
        var users = await _userService.GetAllUsersForReportAsync();

        // PDF create
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

        return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateDepartmentUsersReportAsync(int departmentId)
    {
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

        return document.GeneratePdf();
    }

    public async Task<byte[]> GenerateRoleBasedUsersReportAsync(int roleId)
    {
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
        
        return document.GeneratePdf();
    }

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