namespace UserManagementSystem.Business.Interfaces;

public interface IReportService
{
    Task<byte[]> GenerateAllUsersReportAsync();
    Task<byte[]> GenerateDepartmentUsersReportAsync(int departmentId);
    Task<byte[]> GenerateRoleBasedUsersReportAsync(int roleId);
    Task<byte[]> GenerateUsersByStatusReportAsync(bool isActive);
}