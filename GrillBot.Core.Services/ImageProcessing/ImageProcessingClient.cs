﻿using System.Net.Http.Json;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.ImageProcessing.Models;

namespace GrillBot.Core.Services.ImageProcessing;

public class ImageProcessingClient : RestServiceBase, IImageProcessingClient
{
    public override string ServiceName => "ImageProcessing";

    public ImageProcessingClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<DiagnosticInfo> GetDiagAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/diag", cancellationToken),
            ReadJsonAsync<DiagnosticInfo>,
            timeout: TimeSpan.FromSeconds(60)
        );
    }

    public async Task<byte[]> CreatePeepoloveImageAsync(PeepoRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/image/peepolove", request, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<byte[]> CreatePeepoangryImageAsync(PeepoRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/image/peepoangry", request, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<byte[]> CreatePointsImageAsync(PointsRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/image/points", request, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<byte[]> CreateWithoutAccidentImageAsync(WithoutAccidentImageRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/image/without-accident", request, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<byte[]> CreateChartImageAsync(ChartRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/image/chart", request, cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }
}
