using AutoMapper;
using Common;
using Common.DTOs.ExpenseDTOs;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

namespace Common.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly DigitalKhataDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ExpenseRepository> _logger;
    public ExpenseRepository(DigitalKhataDbContext context, IMapper mapper, ILogger<ExpenseRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ExpenseResponse>> GetAll()
    {
        var expenses = await _context.Expenses.ToListAsync();
        return _mapper.Map<List<ExpenseResponse>>(expenses);
    }

    public async Task<ExpenseResponse> AddExpense(ExpenseAddRequest request)
    {

        var newExpense = _mapper.Map<Expense>(request);
        newExpense.Date = DateTime.Now;

        await _context.Expenses
            .AddAsync(newExpense);

        await _context.SaveChangesAsync();
        var addedExpense = await _context.Expenses.SingleOrDefaultAsync(e => e.ExpenseId == newExpense.ExpenseId);

        return _mapper.Map<ExpenseResponse>(addedExpense);
    }


    public async Task<ExpenseResponse> DeleteExpense(int id)
    {
        var expense = await _context.Expenses.SingleOrDefaultAsync(e => e.ExpenseId == id);
        if (expense != null)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
        return _mapper.Map<ExpenseResponse>(expense);
    }


    public async Task<ExpenseResponse?> GetExpense(int id)
    {
        var expense = await _context.Expenses.SingleOrDefaultAsync(e => e.ExpenseId == id) ?? null;
        return _mapper.Map<ExpenseResponse>(expense);
    }

    public async Task<List<ExpenseResponse>> GetUserTransactions(int userId, int otherUserId)
    {
        var transactions = await _context.Expenses
            .Where(e => (e.PayerUserId == userId && e.PayeeUserId == otherUserId) ||
                        (e.PayerUserId == otherUserId && e.PayeeUserId == userId))
            .ToListAsync();
        return _mapper.Map<List<ExpenseResponse>>(transactions);
    }
}
