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

    protected T GetParameter<T>(int index)
        => (T)Parameters[index]!;

    protected T? GetOptionalParameter<T>(int index)
    {
        var item = Parameters.ElementAtOrDefault(index);
        return item is null ? default : (T)item;
    }
}
