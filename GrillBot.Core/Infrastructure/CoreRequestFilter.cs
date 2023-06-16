using GrillBot.Core.Services.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GrillBot.Core.Infrastructure;

public class RequestFilter : IAsyncActionFilter
{
    private DiagnosticsManager DiagnosticsManager { get; }
    private IEnumerable<IRequestFilterAction> Filters { get; }

    public RequestFilter(DiagnosticsManager diagnosticsManager, IEnumerable<IRequestFilterAction> filters)
    {
        DiagnosticsManager = diagnosticsManager;
        Filters = filters;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var startAt = DateTime.Now;

        SetXssProtection(context.HttpContext.Response);
        foreach (var filter in Filters)
            await filter.BeforeExecutionAsync();

        var result = await next();

        foreach (var filter in Filters)
            await filter.AfterExecutionAsync();

        await DiagnosticsManager.OnRequestEndAsync(context, result, startAt);
    }

    private static void SetXssProtection(HttpResponse response)
    {
        response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
        response.Headers.Add("X-Xss-Protection", "1; mode=block");
        response.Headers.Add("X-Content-Type-Options", "nosniff");
    }
}
