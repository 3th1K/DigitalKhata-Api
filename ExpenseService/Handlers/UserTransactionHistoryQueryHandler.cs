using AutoMapper;
using Common.DTOs.ExpenseDTOs;
using Common.Interfaces;
using Common.Models;
using Ethik.Utility.Api;
using ExpenseService.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Handlers;

public class UserTransactionHistoryQueryHandler : IRequestHandler<UserTransactionHistoryQuery, ApiResult<UserTransactionHistory>>
{
    private readonly ILogger<UserTransactionHistoryQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUserRepository _userRepository;
    public UserTransactionHistoryQueryHandler(ILogger<UserTransactionHistoryQueryHandler> logger, IMapper mapper, IExpenseRepository expenseRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _expenseRepository = expenseRepository;
        _userRepository = userRepository;

    }
    public async Task<ApiResult<UserTransactionHistory>> Handle(UserTransactionHistoryQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(request.UserId);
        var otherUser = await _userRepository.GetUserById(request.OtherUserId);

        if (user == null || otherUser == null)
        {
            _logger.LogError("One or both users not found.");
            return ApiResult<UserTransactionHistory>.Failure(new { }, "One or both users not found.", 404, 7);
        }
        var transactions = await _expenseRepository.GetUserTransactions(request.UserId, request.OtherUserId);

        var netAmountForUser = CalculateNetAmountForUser(transactions, request.UserId);
        var netAmountForOtherUser = CalculateNetAmountForUser(transactions, request.OtherUserId);

        var userTransactionHistory = new UserTransactionHistory();
        userTransactionHistory.Transactions = transactions;
        userTransactionHistory.NetAmountOwedOrReceived = new Dictionary<int, decimal>
        {
            { request.UserId, netAmountForUser },
            { request.OtherUserId, netAmountForOtherUser }
        };

        return ApiResult<UserTransactionHistory>.Success(userTransactionHistory, $"Successfully fetched transaction history of {user.Username} and {otherUser.Username}");
    }
    private decimal CalculateNetAmountForUser(List<ExpenseResponse> transactions, int userId)
    {
        decimal totalPaid = transactions
            .Where(e => e.PayerUserId == userId)
            .Sum(e => e.Amount);

        decimal totalReceived = transactions
            .Where(e => e.PayeeUserId == userId)
            .Sum(e => e.Amount);

        return totalReceived - totalPaid;
    }
}
