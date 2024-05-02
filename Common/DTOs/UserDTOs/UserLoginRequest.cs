using Ethik.Utility.Api;
using MediatR;

namespace Common.DTOs.UserDTOs;

public record UserLoginRequest(string Username, string Password) : IRequest<ApiResult<string>>;
