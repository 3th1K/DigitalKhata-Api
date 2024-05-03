using Common.DTOs.UserDTOs;
using Common.Models;
using AutoMapper;

namespace Common.Profiles;

public class AutomapperProfiles : Profile
{
    public AutomapperProfiles()
    {
        CreateMap<User, UserLoginRequest>();
        CreateMap<UserLoginRequest, User>();

        CreateMap<User, UserRegisterRequest>();
        CreateMap<UserRegisterRequest, User>();
        
        CreateMap<User, UserResponse>();
        CreateMap<UserResponse, User>();
    }
}
