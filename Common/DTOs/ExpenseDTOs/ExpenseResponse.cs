namespace Common.DTOs.ExpenseDTOs;

public class ExpenseResponse
{
    public int ExpenseId { get; set; }

    public int PayerUserId { get; set; }

    public int PayeeUserId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }
}
