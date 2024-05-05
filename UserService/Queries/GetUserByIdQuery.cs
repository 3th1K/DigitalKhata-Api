using Common.DTOs.UserDTOs;
using Ethik.Utility.Api;
using MediatR;

namespace UserService.Queries;

public class GetUserByIdQuery : IRequest<ApiResult<UserResponse>>
{
    public readonly int Id;
    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}
