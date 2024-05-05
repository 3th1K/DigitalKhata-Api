using Ethik.Utility.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common;

public static class ApiResultFactory
{
    public static ApiResult<T> Success<T>(T successObject, string message, int statusCode = 200)
    {
        return ApiResult<T>.Success(successObject, message, statusCode);
    }

    public static ApiResult<T> Failure<T>(string error, string message, int statusCode = 500)
    {
        return ApiResult<T>.Failure(error, message, statusCode);
    }
}

