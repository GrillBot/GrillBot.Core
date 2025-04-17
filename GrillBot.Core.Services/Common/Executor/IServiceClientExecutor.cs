﻿namespace GrillBot.Core.Services.Common.Executor;

public interface IServiceClientExecutor<TServiceInterface> where TServiceInterface : IServiceClient
{
    Task<TResult> ExecuteRequestAsync<TResult>(Func<TServiceInterface, ServiceExecutorContext, Task<TResult>> executeRequest);
    Task ExecuteRequestAsync(Func<TServiceInterface, ServiceExecutorContext, Task> executionRequest);
}
