using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillBot.Core.Infrastructure.Actions;

public class ApiResult
{
    public int StatusCode { get; }
    public object? Data { get; }

    public ApiResult(int statusCode, object? data = null)
    {
        Data = data;
        StatusCode = statusCode;
    }

    public IActionResult ToApiResult()
    {
        if (Data is IActionResult actionResult)
            return actionResult;

        return Data == null ? new StatusCodeResult(StatusCode) : new ObjectResult(Data) { StatusCode = StatusCode };
    }

    public static ApiResult FromSuccess(object? data = null) 
        => new(StatusCodes.Status200OK, data);
}
