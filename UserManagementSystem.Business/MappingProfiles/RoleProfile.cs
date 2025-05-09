using AutoMapper;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.MappingProfiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        // Role -> RoleDto
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));
        
        // RoleAddDto -> Role
        CreateMap<RoleAddDto, Role>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));
        
        // RoleUpdateDto -> Role (For Update)
        CreateMap<RoleUpdateDto, Role>()
            .ForMember(dest => dest.RoleId, opt => opt.Ignore())
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));
    }
}