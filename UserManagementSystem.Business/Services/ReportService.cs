using AutoMapper;
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
    
    public Task<byte[]> GenerateAllUsersReportAsync(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GenerateDepartmentUsersReportAsync(int departmentId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GenerateRoleBasedUsersReportAsync(int roleId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GenerateUsersByStatusReportAsync(bool isActive, int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}