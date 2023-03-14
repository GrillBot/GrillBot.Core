using GrillBot.Core.Database;
using GrillBot.Core.Infrastructure;
using GrillBot.Core.Services.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core;

public static class CoreExtensions
{
    public static IServiceCollection AddDiagnostic(this IServiceCollection services)
    {
        services
            .AddSingleton<DiagnosticsManager>()
            .AddScoped<IDiagnosticsProvider, DiagnosticsProvider>();
        return services;
    }

    public static IServiceCollection AddRequestFilter<TFilter>(this IServiceCollection services) where TFilter : class, IRequestFilterAction
        => services.AddScoped<IRequestFilterAction, TFilter>();

    public static MvcOptions RegisterCoreFilter(this MvcOptions options)
    {
        options.Filters.Add<RequestFilter>();
        return options;
    }

    public static IServiceCollection AddStatisticsProvider<TProvider>(this IServiceCollection services) where TProvider : class, IStatisticsProvider
        => services.AddScoped<IStatisticsProvider, TProvider>();
}
