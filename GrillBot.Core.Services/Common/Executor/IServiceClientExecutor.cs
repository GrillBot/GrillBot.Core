namespace GrillBot.Core.Services.Common.Executor;

public interface IServiceClientExecutor<TServiceInterface> where TServiceInterface : IServiceClient
{
    Task<TResult> ExecuteRequestAsync<TResult>(Func<TServiceInterface, CancellationToken, Task<TResult>> executeRequest);
    Task ExecuteRequestAsync(Func<TServiceInterface, CancellationToken, Task> executionRequest);
}
