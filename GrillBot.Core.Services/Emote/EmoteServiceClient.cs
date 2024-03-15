using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.Emote.Models.Request;
using GrillBot.Core.Services.Emote.Models.Response;
using System.Net.Http.Json;

namespace GrillBot.Core.Services.Emote;

public class EmoteServiceClient : RestServiceBase, IEmoteServiceClient
{
    public override string ServiceName => "Emote";

    public EmoteServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public Task<int> DeleteStatisticsAsync(string guildId, string emoteId, string? userId = null)
    {
        var uri = $"api/statistics/{guildId}/{emoteId}";
        if (!string.IsNullOrEmpty(userId))
            uri += $"?userId={userId}";

        return ProcessRequestAsync(
            cancellationToken => HttpClient.DeleteAsync(uri, cancellationToken),
            ReadJsonAsync<int>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<DiagnosticInfo> GetDiagAsync()
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/diagnostic", cancellationToken),
            ReadJsonAsync<DiagnosticInfo>,
            timeout: TimeSpan.FromSeconds(60)
        );
    }

    public Task<EmoteInfo> GetEmoteInfoAsync(string guildId, string emoteId)
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/emote/{guildId}/{emoteId}/info", cancellationToken),
            ReadJsonAsync<EmoteInfo>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<RestResponse<PaginatedResponse<EmoteStatisticsItem>>> GetEmoteStatisticsListAsync(EmoteStatisticsListRequest request)
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/statistics/emoteStatistics", request, cancellationToken),
            ReadRestResponseAsync<PaginatedResponse<EmoteStatisticsItem>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<bool> GetIsEmoteSupportedAsync(string guildId, string emoteId)
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/emote/supported/{guildId}/{emoteId}", cancellationToken),
            ReadJsonAsync<bool>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<List<string>> GetSupportedEmotesListAsync()
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/emote/supported", cancellationToken),
            ReadJsonAsync<List<string>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<PaginatedResponse<EmoteUserUsageItem>> GetUserEmoteUsageListAsync(EmoteUserUsageListRequest request)
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/statistics/emoteUsersUsage", request, cancellationToken),
            ReadJsonAsync<PaginatedResponse<EmoteUserUsageItem>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<MergeStatisticsResult> MergeStatisticsAsync(string guildId, string sourceEmoteId, string destinationEmoteId)
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.PutAsync($"api/statistics/{guildId}/{sourceEmoteId}/{destinationEmoteId}/merge", null, cancellationToken),
            ReadJsonAsync<MergeStatisticsResult>,
            timeout: TimeSpan.FromSeconds(30)
        );
    }
}
