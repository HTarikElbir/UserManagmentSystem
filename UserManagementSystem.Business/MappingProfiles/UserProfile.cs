using AutoMapper;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Business.Dtos.User;

namespace UserManagementSystem.Business.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // User -> UserDto
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => 
                opt.MapFrom(src => src.UserRoles.Select(r => r.Role).ToList()));
        
        // UpdateUserDto -> User (For Update)
        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        
        // UserAddDto -> User (For Add)
        CreateMap<UserAddDto, User>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => false)) // Set IsActive to true by default
            .ForMember(dest => dest.UserRoles,opt => opt.MapFrom(src => new List<UserRole> 
            {
                new UserRole
                {
                    RoleId = 2 // Default role ID for new users
                }
            }));
        
        // User -> DepartmentUserDto
        CreateMap<User, DepartmentUserDto>()
            .ForMember(dest => dest.Roles, opt => 
                opt.MapFrom(src => src.UserRoles.Select(r => r.Role).ToList()));
            
        
        // UserRoleAddDto -> UserRole
        CreateMap<UserRoleAddDto, UserRole>();
        
       
    }
}