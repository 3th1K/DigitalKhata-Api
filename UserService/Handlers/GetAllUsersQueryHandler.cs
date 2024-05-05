using Common;
using Common.DTOs.UserDTOs;
using Common.Interfaces;
using Ethik.Utility.Api;
using MediatR;
using UserService.Queries;

namespace UserService.Handlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResult<List<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<ApiResult<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsers();
        return ApiResultFactory.Success(users, "Fetched All Users From Database");
    }
}
