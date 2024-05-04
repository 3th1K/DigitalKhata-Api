using AutoMapper;
using Common.DTOs.ExpenseDTOs;
using Common.DTOs.UserDTOs;
using Common.Interfaces;
using Common.Models;
using Ethik.Utility.Api;
using MediatR;

namespace ExpenseService.Handlers;

public class ExpenseAddRequestHandler : IRequestHandler<ExpenseAddRequest, ApiResult<ExpenseResponse>>
{
    private readonly ILogger<ExpenseAddRequestHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUserRepository _userRepository;
    public ExpenseAddRequestHandler(ILogger<ExpenseAddRequestHandler> logger, IMapper mapper, IExpenseRepository expenseRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _expenseRepository = expenseRepository;
        _userRepository = userRepository;

    }
    public async Task<ApiResult<ExpenseResponse>> Handle(ExpenseAddRequest request, CancellationToken cancellationToken)
    {
        UserResponse payer = await _userRepository.GetUserById(request.PayerUserId);

        if (payer == null) 
        {
            _logger.LogError("Expense Add Unsuccessful, invalid payer id");
            return ApiResult<ExpenseResponse>.Failure(new { }, "Invalid payer id", 500, 3);
        }

        UserResponse payee = await _userRepository.GetUserById(request.PayeeUserId);

        if (payee == null)
        {
            _logger.LogError("Expense Add Unsuccessful, invalid payee id");
            return ApiResult<ExpenseResponse>.Failure(new { }, "Invalid payee id", 500, 4);
        }

        if(request.Amount<1)
        {
            _logger.LogError("Expense Add Unsuccessful, invalid amount, amount cannot be less that 1");
            return ApiResult<ExpenseResponse>.Failure(new { }, "Invalid amount, amount cannot be less that 1", 500, 5);
        }

        ExpenseResponse createdExpense = await _expenseRepository.AddExpense(request);
        if (createdExpense == null) 
        {
            _logger.LogError("Expense Add Unsuccessful");
            return ApiResult<ExpenseResponse>.Failure(new { }, "Expense was not added", 500, 6);
        }
        _logger.LogInformation("Expense Add Successful");
        return ApiResult<ExpenseResponse>.Success(createdExpense, "Expense was added");
    }
}
