using Ethik.Utility.Error;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Ethik.Utility.Api;

public class ApiResult<T>
{
    public T? Data { get; private set; } = default(T);
    public ErrorInfo? Error { get; private set; } = null;
    public ObjectResult Result { get; private set; }
    public bool IsSuccess { get; private set; }

    public ApiResult(bool success, object data, ObjectResult result)
    {
        if (success) 
        {
            Data = (T)data;
        }
        else
        {
            Error = (ErrorInfo)data;
        }
        Result = result;
        IsSuccess = success;
    }

    public static ApiResult<T> Success(T successObject, string message, int statusCode=200) 
    {
        ApiResponse<T> response = new ApiResponse<T>()
        {
            Success = true,
            Message = message,
            Data = successObject
        };
        ObjectResult result = new ObjectResult(response) { StatusCode = statusCode };
        return new ApiResult<T>(true, successObject, result);
    }

    public static ApiResult<T> Failure(ErrorInfo failureObject, string message, int statusCode = 500)
    {
        ApiResponse<ErrorInfo> response = new ApiResponse<ErrorInfo>()
        {
            Success = false,
            Message = message,
            Error = failureObject
        };
        ObjectResult result = new ObjectResult(response) { StatusCode = statusCode };
        return new ApiResult<T>(false, failureObject, result);
    }

    public static ApiResult<T> Failure(string error, string message, int statusCode = 500)
    {
        Assembly callingAssembly = Assembly.GetCallingAssembly();
        ErrorResourceHelper resourceHelper = new ErrorResourceHelper(callingAssembly, "ApiErrors");
        ErrorInfo errorObject = resourceHelper.GetResourceInfo(error);
        
        return Failure(errorObject, message, statusCode);
    }
}
