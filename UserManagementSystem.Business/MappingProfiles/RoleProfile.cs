using AutoMapper;
using UserManagementSystem.Business.Dtos;
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
        
        // RoleDto -> Role
        CreateMap<RoleDto, Role>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));
    }
}