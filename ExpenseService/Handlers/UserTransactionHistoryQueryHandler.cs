using AutoMapper;
using Common;
using Common.DTOs.ExpenseDTOs;
using Common.DTOs.UserDTOs;
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
        if (request.UserId == request.OtherUserId) 
        {
            _logger.LogError("Both user id's are same");
            return ApiResultFactory.Failure<UserTransactionHistory>(ErrorConstants.SameUsersInTransactionHistory, "Both user id's are same", 400);
        }
        var user = await _userRepository.GetUserById(request.UserId);
        var otherUser = await _userRepository.GetUserById(request.OtherUserId);

        if (user == null || otherUser == null)
        {
            _logger.LogError("One or both users not found.");
            return ApiResultFactory.Failure<UserTransactionHistory>(ErrorConstants.OneOrBothUserNotFound, "One or both users not found.", 404);
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

        return ApiResultFactory.Success(userTransactionHistory, $"Successfully fetched transaction history of {user.Username} and {otherUser.Username}");
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

public class UserExpenseQueryHandler : IRequestHandler<UserExpensesQuery, ApiResult<List<UserResponse>>>
{
    private readonly ILogger<UserExpenseQueryHandler> _logger;
    private readonly IExpenseRepository _expenseRepository;

    public UserExpenseQueryHandler(ILogger<UserExpenseQueryHandler> logger, IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
        _logger = logger;
    }
    public async Task<ApiResult<List<UserResponse>>> Handle(UserExpensesQuery request, CancellationToken cancellationToken)
    {
        List<UserResponse> users = await _expenseRepository.GetUsersWithExpenses(request.UserId);
        if (users == null || users.Count < 1)
        {
            _logger.LogError("User does not have any expenses");
            return ApiResultFactory.Failure<List<UserResponse>>(ErrorConstants.NoUserExpenseFound, "No user expense was found");
        }
        else 
        {
            _logger.LogInformation("Fetched user expenses");
            return ApiResultFactory.Success(users, "Fetched user expenses");
        }
    }
}
