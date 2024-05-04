using Common.DTOs.UserDTOs;
using Common.Models;
using AutoMapper;
using Common.DTOs.ExpenseDTOs;

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

        CreateMap<ExpenseAddRequest, Expense>();
        CreateMap<Expense, ExpenseAddRequest>();

        CreateMap<Expense, ExpenseResponse>();
        CreateMap<ExpenseResponse, Expense>();
    }
}
