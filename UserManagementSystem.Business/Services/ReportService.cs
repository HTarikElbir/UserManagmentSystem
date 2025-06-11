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
    private readonly IMapper _mapper;

    public ReportService(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
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

    public Task<byte[]> GenerateDepartmentUsersReportAsync(int departmentId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GenerateRoleBasedUsersReportAsync(int roleId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GenerateUsersByStatusReportAsync(bool isActive)
    {
        throw new NotImplementedException();
    }
    
    private void ComposeHeader(IContainer container, string title)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text(title).FontSize(20).SemiBold();
                column.Item().Text($"Generated on: {DateTime.Now:g}").FontSize(10);
            });
        });
    }

    private void ComposeContent(IContainer container, List<UserDto> users)
    {
        container.Table(table =>
        {
            // Tablo başlıkları
            table.Header(header =>
            {
                header.Cell().Text("ID").FontSize(10).SemiBold();
                header.Cell().Text("Username").FontSize(10).SemiBold();
                header.Cell().Text("Email").FontSize(10).SemiBold();
                header.Cell().Text("Department").FontSize(10).SemiBold();
                header.Cell().Text("Roles").FontSize(10).SemiBold();
            });

            // Tablo içeriği
            foreach (var user in users)
            {
                table.Cell().Text(user.UserId.ToString());
                table.Cell().Text(user.Username);
                table.Cell().Text(user.Email);
                table.Cell().Text(user.Department);
                table.Cell().Text(string.Join(", ", user.Roles.Select(r => r.RoleName)));
            }
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Text(text =>
            {
                text.Span("Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        });
    }
}