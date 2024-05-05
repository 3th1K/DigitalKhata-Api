using AutoMapper;
using Common;
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
            return ApiResultFactory.Failure<ExpenseResponse>(ErrorConstants.InvalidPayerId, "Invalid payer id", 400);
        }

        UserResponse payee = await _userRepository.GetUserById(request.PayeeUserId);

        if (payee == null)
        {
            _logger.LogError("Expense Add Unsuccessful, invalid payee id");
            return ApiResultFactory.Failure<ExpenseResponse>(ErrorConstants.InvalidPayeeId, "Invalid payee id", 400);
        }

        if(request.Amount<1)
        {
            _logger.LogError("Expense Add Unsuccessful, invalid amount, amount cannot be less that 1");
            return ApiResultFactory.Failure<ExpenseResponse>(ErrorConstants.InvalidExpenseAmount, "Invalid amount, amount cannot be less that 1", 400);
        }

        ExpenseResponse createdExpense = await _expenseRepository.AddExpense(request);
        if (createdExpense == null) 
        {
            _logger.LogError("Expense Add Unsuccessful");
            return ApiResultFactory.Failure<ExpenseResponse>(ErrorConstants.AddExpenseFailure, "Expense was not added", 500);
        }
        _logger.LogInformation("Expense Add Successful");
        return ApiResultFactory.Success(createdExpense, "Expense was added");
    }
}
