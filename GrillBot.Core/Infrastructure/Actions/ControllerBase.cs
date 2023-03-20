using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.Infrastructure.Actions;

[ApiController]
[Route("api/[controller]")]
public abstract class ControllerBase
{
    protected IServiceProvider ServiceProvider { get; }

    protected ControllerBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    protected async Task<IActionResult> ProcessAsync<TAction>(params object?[] parameters) where TAction : IApiAction
    {
        var action = ServiceProvider.GetRequiredService<TAction>();
        var result = await action.ProcessAsync(parameters);

        return result.ToApiResult();
    }
}
