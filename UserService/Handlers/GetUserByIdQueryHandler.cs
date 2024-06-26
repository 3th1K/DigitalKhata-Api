﻿using Common;
using Common.DTOs.UserDTOs;
using Common.Interfaces;
using Ethik.Utility.Api;
using MediatR;
using UserService.Queries;

namespace UserService.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResult<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    public GetUserByIdQueryHandler(IUserRepository userRepository, ILogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    public async Task<ApiResult<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.Id);
        if (user == null)
        {
            _logger.LogError($"User with id {request.Id} not found");
            return ApiResultFactory.Failure<UserResponse>(ErrorConstants.InvalidUserId, "Invalid User Id", 404);
        }
        _logger.LogInformation("Fetching user details successful");
        return ApiResultFactory.Success(user, "Fetched User Details");
    }
}
