using GrillBot.Core.Extensions;
using GrillBot.Core.Services.AuditLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.Services;

public static class ServicesExtensions
{
    private static void AddHttpClient(this IServiceCollection services, IConfiguration configuration, string serviceId)
    {
        services.AddHttpClient(serviceId, client =>
        {
            client.BaseAddress = new Uri(configuration[$"Services:{serviceId}:Api"]!);
            client.Timeout = TimeSpan.FromMilliseconds(configuration[$"Services:{serviceId}:Timeout"]!.ToInt());
        });
    }

    private static void AddService<TInterface, TImplementation>(this IServiceCollection services, IConfiguration configuration, string serviceName)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services
            .AddScoped<TInterface, TImplementation>()
            .AddHttpClient(configuration, serviceName);
    }

    public static void AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddService<IAuditLogServiceClient, AuditLogServiceClient>(configuration, "AuditLog");
    }
}
