using GrillBot.Core.Services.Common.Attributes;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace GrillBot.Core.Services.Common.Executor;

public class ServiceClientExecutor<TServiceInterface>(
    IConfiguration _configuration,
    TServiceInterface _serviceClient
) : IServiceClientExecutor<TServiceInterface> where TServiceInterface : IServiceClient
{
    public async Task<TResult> ExecuteRequestAsync<TResult>(Func<TServiceInterface, CancellationToken, Task<TResult>> executeRequest)
    {
        using var cancellationTokenSource = CreateCancellationToken();
        return await executeRequest(_serviceClient, cancellationTokenSource.Token);
    }

    public async Task ExecuteRequestAsync(Func<TServiceInterface, CancellationToken, Task> executionRequest)
    {
        using var cancellationTokenSource = CreateCancellationToken();
        await executionRequest(_serviceClient, cancellationTokenSource.Token);
    }

    private CancellationTokenSource CreateCancellationToken()
    {
        var timeout = GetTimeout();
        return new CancellationTokenSource(timeout);
    }

    private TimeSpan GetTimeout()
    {
        var serviceName = typeof(TServiceInterface).GetCustomAttribute<ServiceAttribute>()!.ServiceName;
        return _configuration.GetValue<TimeSpan>($"Services:{serviceName}:Timeout");
    }
}
