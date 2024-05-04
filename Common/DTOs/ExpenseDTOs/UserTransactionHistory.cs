namespace Common.DTOs.ExpenseDTOs;

public class UserTransactionHistory
{
    public List<ExpenseResponse> Transactions { get; set; } = new List<ExpenseResponse>();
    public Dictionary<int, decimal> NetAmountOwedOrReceived { get; set; } = new Dictionary<int, decimal>();
}

