using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ;

public class RabbitMQHealthCheck : IHealthCheck
{
    private IConnection Connection { get; }

    public RabbitMQHealthCheck(IConnection connection)
    {
        Connection = connection;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var model = Connection.CreateModel();
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
        }
    }
}
