using Common.DTOs.ExpenseDTOs;
using Common.Models;
using ExpenseService.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExpenseController> _logger;
    public ExpenseController(IMediator mediator, ILogger<ExpenseController> logger)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddExpense(ExpenseAddRequest expenseAddRequest)
    {
        var expense = await _mediator.Send(expenseAddRequest);
        return expense.Result;
    }

    [HttpGet]
    [Route("{userId}/transaction-history/{otherUserId}")]
    public async Task<IActionResult> GetUserTransactionHistory(int userId, int otherUserId)
    {
        var transactionHistory = await _mediator.Send(new UserTransactionHistoryQuery(userId, otherUserId));
        return transactionHistory.Result;
    }

    [HttpGet]
    [Route("expense-users/{id}")]
    public async Task<IActionResult> GetUserExpenses(int id)
    {
        var userExpenses = await _mediator.Send(new UserExpensesQuery(id));
        return userExpenses.Result;
    }

    

}
