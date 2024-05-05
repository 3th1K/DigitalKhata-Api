using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common;

public class ErrorConstants
{
    public const string TokenGenerationFailed = "err_token_generation_failed";
    public const string UserAlreadyExists = "err_user_exists";
    public const string InvalidPayerId = "err_invalid_payer_id";
    public const string InvalidPayeeId = "err_invalid_payee_id";
    public const string InvalidExpenseAmount = "err_invalid_expense_amount";
    public const string AddExpenseFailure = "err_add_expense_failure";
}
