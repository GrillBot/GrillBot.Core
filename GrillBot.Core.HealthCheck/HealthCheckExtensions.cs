using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.HealthCheck;

public static class HealthCheckExtensions
{
    public static IHealthChecksBuilder AddHealthChecks(this IServiceCollection services)
    {
        return HealthCheckServiceCollectionExtensions.AddHealthChecks(services);
    }

    public static IEndpointConventionBuilder MapHealthChecks(this IEndpointRouteBuilder endpoints, string pattern = "/health")
    {
        var options = new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = HealthCheckWriter.WriteResponseAsync
        };

        return HealthCheckEndpointRouteBuilderExtensions.MapHealthChecks(endpoints, pattern, options);
    }
}
