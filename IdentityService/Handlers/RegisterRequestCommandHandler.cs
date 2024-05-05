using AutoMapper;
using Common;
using Common.DTOs.UserDTOs;
using Common.Interfaces;
using Common.Models;
using Ethik.Utility.Api;
using MediatR;

namespace IdentityService.Handlers;

public class RegisterRequestCommandHandler : IRequestHandler<UserRegisterRequest, ApiResult<string>>
{
    private readonly IIdentityRepository _repository;
    private readonly ILogger<RegisterRequestCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public RegisterRequestCommandHandler(IIdentityRepository repository, ILogger<RegisterRequestCommandHandler> logger, IMapper mapper, IUserRepository userRepository)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    public async Task<ApiResult<string>> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        User user = _mapper.Map<User>(request);
        var existingUser = await _userRepository.GetUserByUsernameOrEmail(user.Username, user.Email);
        if (existingUser != null)
        {
            _logger.LogError("user already exists with same username or email");
            return ApiResultFactory.Failure<string>(ErrorConstants.UserAlreadyExists, "User already exists with the provided username or email", 409);
        }

        var addedUser = await _userRepository.CreateUser(user);
        _logger.LogInformation("User created {}", addedUser);

        var token = await _repository.GetToken(request.Username, request.Password);
        if (token == string.Empty)
        {
            _logger.LogError("Token Generation Failed");
            return ApiResultFactory.Failure<string>(ErrorConstants.TokenGenerationFailed, "Incorrect Credentials", 401);
        }

        _logger.LogInformation("User Registration Successful");
        return ApiResultFactory.Success(token, "User Registration Successful & Token Generation Successful");
    }
}