namespace UserManagementSystem.Business.Interfaces;

public interface IReportService
{
    Task<byte[]> GenerateAllUsersReportAsync(int page, int pageSize);
    Task<byte[]> GenerateDepartmentUsersReportAsync(int departmentId, int page, int pageSize);
    Task<byte[]> GenerateRoleBasedUsersReportAsync(int roleId, int page, int pageSize);
    Task<byte[]> GenerateUsersByStatusReportAsync(bool isActive, int page, int pageSize);
}