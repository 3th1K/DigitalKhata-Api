using Common.DTOs.ExpenseDTOs;
using Ethik.Utility.Api;
using MediatR;

namespace ExpenseService.Queries;

public class UserTransactionHistoryQuery : IRequest<ApiResult<UserTransactionHistory>>
{
    public readonly int UserId;
    public readonly int OtherUserId;
    public UserTransactionHistoryQuery(int userId, int otherUserId)
    {
        UserId = userId;
        OtherUserId = otherUserId;
    }
}
