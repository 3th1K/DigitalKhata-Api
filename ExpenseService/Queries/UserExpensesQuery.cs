using Common.DTOs.UserDTOs;
using Ethik.Utility.Api;
using MediatR;

namespace ExpenseService.Queries;

public class UserExpensesQuery : IRequest<ApiResult<List<UserResponse>>>
{
    public readonly int UserId;
    public UserExpensesQuery(int userId)
    {
        UserId = userId;
    }
}
