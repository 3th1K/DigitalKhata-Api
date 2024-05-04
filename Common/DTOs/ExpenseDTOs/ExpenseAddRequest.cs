using Ethik.Utility.Api;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.ExpenseDTOs;

public class ExpenseAddRequest : IRequest<ApiResult<ExpenseResponse>>
{
    public int PayerUserId { get; set; }

    public int PayeeUserId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }
}
