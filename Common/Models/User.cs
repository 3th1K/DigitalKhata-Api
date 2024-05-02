using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Fullname { get; set; }

    public virtual ICollection<Expense> ExpensePayeeUsers { get; set; } = new List<Expense>();

    public virtual ICollection<Expense> ExpensePayerUsers { get; set; } = new List<Expense>();
}
