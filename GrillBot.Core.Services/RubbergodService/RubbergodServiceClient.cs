using System.Net.Http.Json;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.RubbergodService.Models.Help;
using GrillBot.Core.Services.RubbergodService.Models.Karma;

namespace GrillBot.Core.Services.RubbergodService;

public class RubbergodServiceClient : RestServiceBase, IRubbergodServiceClient
{
    public override string ServiceName => "RubbergodService";

    public RubbergodServiceClient(ICounterManager counterManager, IHttpClientFactory clientFactory) : base(counterManager, clientFactory)
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

    public async Task<PaginatedResponse<UserKarma>> GetKarmaPageAsync(PaginatedParams parameters)
    {
        var query = $"Page={parameters.Page}&PageSize={parameters.PageSize}";
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/karma?{query}", cancellationToken),
            ReadJsonAsync<PaginatedResponse<UserKarma>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task StoreKarmaAsync(List<KarmaItem> items)
    {
        await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/karma", items, cancellationToken),
            EmptyResponseAsync,
            timeout: TimeSpan.FromSeconds(30)
        );
    }

    public async Task InvalidatePinCacheAsync(ulong guildId, ulong channelId)
    {
        await ProcessRequestAsync(
            cancellationToken => HttpClient.DeleteAsync($"api/pins/{guildId}/{channelId}", cancellationToken),
            EmptyResponseAsync,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<byte[]> GetPinsAsync(ulong guildId, ulong channelId, bool markdown)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/pins/{guildId}/{channelId}?markdown={markdown}", cancellationToken),
            (response, cancellationToken) => response.Content.ReadAsByteArrayAsync(cancellationToken: cancellationToken)!,
            timeout: TimeSpan.FromMinutes(5)
        );
    }

    public async Task<Dictionary<string, Cog>> GetSlashCommandsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/help/slashcommands", cancellationToken),
            ReadJsonAsync<Dictionary<string, Cog>>,
            timeout: TimeSpan.FromSeconds(60)
        );
    }
}
