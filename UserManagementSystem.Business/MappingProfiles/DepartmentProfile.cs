using AutoMapper;
using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.MappingProfiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        // Department -> DepartmentDto
        CreateMap<Department, DepartmentDto>();
    }
}