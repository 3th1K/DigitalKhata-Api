
using Common.DTOs.ExpenseDTOs;
using Common.DTOs.UserDTOs;

namespace Common.Interfaces;

public interface IExpenseRepository
{
    public Task<List<ExpenseResponse>> GetAll();
    public Task<ExpenseResponse> AddExpense(ExpenseAddRequest request);
    public Task<ExpenseResponse> DeleteExpense(int id);
    public Task<ExpenseResponse?> GetExpense(int id);
    public Task<List<ExpenseResponse>> GetUserTransactions(int userId, int otherUserId);
    public Task<List<UserResponse>> GetUsersWithExpenses(int userId);
}
