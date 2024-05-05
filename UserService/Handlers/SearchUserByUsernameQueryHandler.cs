using AutoMapper;
using Common;
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
            return ApiResultFactory.Failure<List<UserResponse>>(ErrorConstants.SearchUserNotFound, "No Users Found", 404);
        }
        _logger.LogInformation("{} users found in search", users.Count);
        return ApiResultFactory.Success(users, $"{users.Count} users found in search");
    }
}
