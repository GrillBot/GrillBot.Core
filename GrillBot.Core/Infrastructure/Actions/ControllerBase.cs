﻿using GrillBot.Core.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.Infrastructure.Actions;

[ApiController]
[Route("api/[controller]")]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected IServiceProvider ServiceProvider { get; }

    protected ControllerBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    protected async Task<IActionResult> ProcessAsync<TAction>(params object?[] parameters) where TAction : ApiActionBase
    {
        var action = ServiceProvider.GetRequiredService<TAction>();
        var currentUser = ServiceProvider.GetRequiredService<ICurrentUserProvider>();

        action.Init(HttpContext, parameters, currentUser);
        var result = await action.ProcessAsync();

        return result.ToApiResult();
    }
}
