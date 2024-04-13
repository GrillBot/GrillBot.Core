using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.Emote.Models.Request;
using GrillBot.Core.Services.Emote.Models.Response;

namespace GrillBot.Core.Services.Emote;

public class EmoteServiceClient : RestServiceBase, IEmoteServiceClient
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(1);

    public override string ServiceName => "Emote";

    public EmoteServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<int> DeleteStatisticsAsync(string guildId, string emoteId, string? userId = null)
    {
        var uri = $"api/statistics/{guildId}/{emoteId}";
        if (!string.IsNullOrEmpty(userId))
            uri += $"?userId={userId}";

        return (await ProcessRequestAsync<int>(() => HttpMethod.Delete.ToRequest(uri), _defaultTimeout))!;
    }

    public async Task<EmoteInfo> GetEmoteInfoAsync(string guildId, string emoteId)
        => (await ProcessRequestAsync<EmoteInfo>(() => HttpMethod.Get.ToRequest($"api/emote/{guildId}/{emoteId}/info"), _defaultTimeout))!;

    public async Task<PaginatedResponse<EmoteStatisticsItem>> GetEmoteStatisticsListAsync(EmoteStatisticsListRequest request)
        => (await ProcessRequestAsync<PaginatedResponse<EmoteStatisticsItem>>(() => HttpMethod.Post.ToRequest("api/statistics/emoteStatistics", request), _defaultTimeout))!;

    public async Task<List<EmoteDefinition>> GetSupportedEmotesListAsync(string? guildId = null)
    {
        var uri = "api/emote/supported";
        if (!string.IsNullOrEmpty(guildId))
            uri += $"?guildId={guildId}";

        return (await ProcessRequestAsync<List<EmoteDefinition>>(() => HttpMethod.Get.ToRequest(uri), _defaultTimeout))!;
    }

    public async Task<PaginatedResponse<EmoteUserUsageItem>> GetUserEmoteUsageListAsync(EmoteUserUsageListRequest request)
        => (await ProcessRequestAsync<PaginatedResponse<EmoteUserUsageItem>>(() => HttpMethod.Post.ToRequest("api/statistics/emoteUsersUsage", request), _defaultTimeout))!;

    public async Task<MergeStatisticsResult> MergeStatisticsAsync(string guildId, string sourceEmoteId, string destinationEmoteId)
        => (await ProcessRequestAsync<MergeStatisticsResult>(() => HttpMethod.Put.ToRequest($"api/statistics/{guildId}/{sourceEmoteId}/{destinationEmoteId}/merge"), _defaultTimeout))!;

    public async Task<long> GetStatisticsCountInGuildAsync(string guildId)
        => await ProcessRequestAsync<long>(() => HttpMethod.Get.ToRequest($"api/statistics/count/{guildId}"), _defaultTimeout);
}
