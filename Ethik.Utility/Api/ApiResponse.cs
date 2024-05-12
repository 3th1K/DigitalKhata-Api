using Ethik.Utility.Error;
using System.Text.Json.Serialization;

namespace Ethik.Utility.Api;

public class ApiResponse<T>
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "No message";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; } = default(T?);

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ErrorInfo? Error { get; set; } = null;
}
