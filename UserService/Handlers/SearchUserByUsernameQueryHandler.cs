using AutoMapper;
using Common.DTOs.UserDTOs;
using Common.Interfaces;
using Ethik.Utility.Api;
using MediatR;
using UserService.Queries;

namespace UserService.Handlers;

public class SearchUserByUsernameQueryHandler : IRequestHandler<SearchUserByUsernameQuery, ApiResult<List<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<SearchUserByUsernameQueryHandler> _logger;
    private readonly IMapper _mapper;
    public SearchUserByUsernameQueryHandler(IUserRepository userRepository, ILogger<SearchUserByUsernameQueryHandler> logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<ApiResult<List<UserResponse>>> Handle(SearchUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        List<UserResponse> users = await _userRepository.SearchUserByUsername(request.SearchString);
        if (users == null || users.Count<1)
        {
            _logger.LogWarning("No user found containing query : {}", request.SearchString);
            return ApiResult<List<UserResponse>>.Failure(new { }, "No Users Found", 404, 2);
        }
        _logger.LogInformation("{} users found in search", users.Count);
        return ApiResult<List<UserResponse>>.Success(users, $"{users.Count} users found in search");
    }
}
