using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

    public static ApiResult Ok(object? data = null)
        => new(StatusCodes.Status200OK, data);

    public static ApiResult NotFound(object? data = null)
        => new(StatusCodes.Status404NotFound, data);

    public static ApiResult BadRequest()
        => new(StatusCodes.Status400BadRequest);

    public static ApiResult BadRequest(ModelStateDictionary? modelState)
        => BadRequest(modelState is null ? null : new ValidationProblemDetails(modelState));

    public static ApiResult BadRequest(ValidationProblemDetails? validationErrors)
        => new(StatusCodes.Status400BadRequest, validationErrors);
}
