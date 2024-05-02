using Common.DTOs.UserDTOs;
using Common.Interfaces;
using Ethik.Utility.Api;
using MediatR;

namespace IdentityService.Handlers;

public class LoginRequestCommandHandler : IRequestHandler<UserLoginRequest, ApiResult<string>>
{
    private readonly IIdentityRepository _repository;
    private readonly ILogger<LoginRequestCommandHandler> _logger;
    public LoginRequestCommandHandler(IIdentityRepository repository, ILogger<LoginRequestCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<ApiResult<string>> Handle(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var token = await _repository.GetToken(request.Username, request.Password);
        if (token == string.Empty) 
        {
            _logger.LogError("Token Generation Failed");
            return ApiResult<string>.Failure(null, "Incorrect Credentials", 401, 001);
        }
        _logger.LogInformation("Token Generated Successfully");
        return ApiResult<string>.Success(token, "Token Generation Successful");
    }
}
