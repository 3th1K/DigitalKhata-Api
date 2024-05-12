namespace Ethik.Utility.Error;

public class ErrorInfo
{
    public string Error { get; set; } = string.Empty;
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string ErrorDescription { get; set; } = string.Empty;
    public string ErrorSolution { get; set; } = string.Empty;
}
