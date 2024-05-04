
using Common.DTOs.ExpenseDTOs;

namespace Common.Interfaces;

public interface IExpenseRepository
{
    public Task<List<ExpenseResponse>> GetAll();
    public Task<ExpenseResponse> AddExpense(ExpenseAddRequest request);
    public Task<ExpenseResponse> DeleteExpense(int id);
    public Task<ExpenseResponse?> GetExpense(int id);
}
