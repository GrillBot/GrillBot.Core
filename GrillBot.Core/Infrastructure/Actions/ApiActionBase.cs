using GrillBot.Core.Infrastructure.Auth;
using Microsoft.AspNetCore.Http;

namespace GrillBot.Core.Infrastructure.Actions;

public abstract class ApiActionBase
{
    protected HttpContext HttpContext { get; private set; } = null!;
    protected object?[] Parameters { get; set; } = null!;
    protected ICurrentUserProvider CurrentUser { get; private set; } = null!;

    public void Init(HttpContext httpContext, object?[] parameters, ICurrentUserProvider currentUser)
    {
        Parameters = parameters;
        HttpContext = httpContext;
        CurrentUser = currentUser;
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
