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
        
        // DepartmentAddDto -> Department
        CreateMap<DepartmentAddDto, Department>();
        
        // DepartmentUpdateDto -> Department
        CreateMap<DepartmentUpdateDto, Department>()
            .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore());
    }
}