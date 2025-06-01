using AutoMapper;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.MappingProfiles;

public class UserRoleProfile : Profile
{
    
    public UserRoleProfile()
    {
        // AssignRoleDto -> UserRole
        CreateMap<AssignRoleDto, UserRole>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.User, opt => opt.Ignore()) 
            .ForMember(dest => dest.Role, opt => opt.Ignore());
        
        // RemoveRoleDto -> UserRole
        CreateMap<RemoveRoleDto, UserRole>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());
    }
}