using Discord;
using GrillBot.Core.Database;
using GrillBot.Core.Infrastructure;
using GrillBot.Core.Managers.Discord;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Managers.Random;
using GrillBot.Core.Services.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public static IServiceCollection AddCoreManagers(this IServiceCollection services)
    {
        services.AddSingleton<ICounterManager, CounterManager>();
        services.AddScoped<IEmoteManager, EmoteManager>();
        services.AddSingleton<IRandomManager, RandomManager>();

        return services;
    }

    public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services, Func<DbContextOptionsBuilder, DbContextOptionsBuilder> useProvider) where TContext : DbContext
        => services.AddDbContext<TContext>(builder => useProvider(builder).EnableDetailedErrors().EnableThreadSafetyChecks(), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

    public static void InitDatabase<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }

    public static async Task InitDatabaseAsync<TContext>(this IApplicationBuilder app) where TContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
            await context.Database.MigrateAsync();
    }

    /// <summary>
    /// Adds fake discord client for correct loading of managers using IDiscordClient.
    /// </summary>
    public static IServiceCollection AddFakeDiscordClient(this IServiceCollection services, ServiceLifetime lifetime)
        => services.AddSingleton<IDiscordClient>(_ => null!);
}
