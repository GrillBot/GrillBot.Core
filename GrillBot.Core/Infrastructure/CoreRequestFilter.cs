using GrillBot.Core.Services.Diagnostics;
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
        await Task.WhenAll(Filters.Select(o => o.BeforeExecutionAsync()));

        var result = await next();

        await Task.WhenAll(Filters.Select(o => o.AfterExecutionAsync()));
        await DiagnosticsManager.OnRequestEndAsync(context, result, startAt);
    }
}
