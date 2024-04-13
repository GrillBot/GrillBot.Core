using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.Diagnostics.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrillBot.Core.Services.Common;

public abstract class RestServiceBase
{
    private readonly HttpClient _client;
    private readonly ICounterManager _counterManager;
    public abstract string ServiceName { get; }

    public string Url => _client.BaseAddress!.ToString();

    protected RestServiceBase(ICounterManager counterManager, IHttpClientFactory httpClientFactory)
    {
        _counterManager = counterManager;
        _client = httpClientFactory.CreateClient(ServiceName);
    }

    protected async Task<TResult?> ProcessRequestAsync<TResult>(Func<HttpRequestMessage> createRequest, TimeSpan timeout)
    {
        using (_counterManager.Create($"Service.{ServiceName}"))
        {
            using var cancellationTokenSource = new CancellationTokenSource(timeout);
            using var request = createRequest();
            using var response = await ExecuteRequestAsync(request, cancellationTokenSource.Token);

            await CheckResponseAsync(response, cancellationTokenSource.Token);
            return await FetchResultAsync<TResult>(response, cancellationTokenSource.Token);
        }
    }

    protected async Task ProcessRequestAsync(Func<HttpRequestMessage> createRequest, TimeSpan timeout)
    {
        using (_counterManager.Create($"Service.{ServiceName}"))
        {
            using var cancellationTokenSource = new CancellationTokenSource(timeout);
            using var request = createRequest();
            using var response = await ExecuteRequestAsync(request, cancellationTokenSource.Token);

            await CheckResponseAsync(response, cancellationTokenSource.Token);
        }
    }

    protected async Task<byte[]> ProcessRequestWithFileAsync(Func<HttpRequestMessage> createRequest, TimeSpan timeout)
    {
        using (_counterManager.Create($"Service.{ServiceName}"))
        {
            using var cancellationTokenSource = new CancellationTokenSource(timeout);
            using var request = createRequest();
            using var response = await ExecuteRequestAsync(request, cancellationTokenSource.Token);

            await CheckResponseAsync(response, cancellationTokenSource.Token);
            return await response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationTokenSource.Token);
        }
    }

    private async Task<HttpResponseMessage> ExecuteRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken, bool isRepeat = false)
    {
        try
        {
            return await _client.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.InnerException is SocketException socketException &&
                                              (socketException.NativeErrorCode == 111 || socketException.SocketErrorCode == SocketError.ConnectionRefused) && !isRepeat)
        {
            await Task.Delay(1000); // Wait 1 second to repeat request execution.
            return await ExecuteRequestAsync(request, cancellationToken, true);
        }
    }

    private static async Task CheckResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
            throw new ClientBadRequestException(problemDetails!);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new ClientNotFoundException();

        if (response.StatusCode == HttpStatusCode.NotAcceptable)
            throw new ClientNotAcceptableException();

        var content = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        throw new ClientException(response.StatusCode, content);
    }

    private static async Task<TResult?> FetchResultAsync<TResult>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.StatusCode == HttpStatusCode.NoContent)
            return default;

        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken);
    }

    public async Task<bool> IsHealthyAsync()
    {
        try
        {
            await ProcessRequestAsync(() => HttpMethod.Head.ToRequest("health"), TimeSpan.FromSeconds(30));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public virtual async Task<DiagnosticInfo> GetDiagnosticAsync()
        => (await ProcessRequestAsync<DiagnosticInfo>(() => HttpMethod.Get.ToRequest("api/diag"), TimeSpan.FromMinutes(1)))!;
}
