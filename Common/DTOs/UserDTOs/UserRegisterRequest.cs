using Ethik.Utility.Api;
using MediatR;

namespace Common.DTOs.UserDTOs;

public record UserRegisterRequest(string Username, string Email, string Password, string Fullname) : IRequest<ApiResult<string>>;
