using Microsoft.AspNetCore.Http;

namespace GrillBot.Core.Infrastructure.Actions;

public abstract class ApiActionBase
{
    protected HttpContext HttpContext { get; private set; } = null!;
    protected object?[] Parameters { get; set; } = null!;

    public void Init(HttpContext httpContext, object?[] parameters)
    {
        Parameters = parameters;
        HttpContext = httpContext;
    }

    public abstract Task<ApiResult> ProcessAsync();
}
