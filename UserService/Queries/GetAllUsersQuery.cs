using Common.DTOs.UserDTOs;
using Ethik.Utility.Api;
using MediatR;

namespace UserService.Queries;

public record GetAllUsersQuery : IRequest<ApiResult<List<UserResponse>>>;
