using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public int PayerUserId { get; set; }

    public int PayeeUserId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public virtual User PayeeUser { get; set; } = null!;

    public virtual User PayerUser { get; set; } = null!;
}
