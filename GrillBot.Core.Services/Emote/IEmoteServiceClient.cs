using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.Emote.Models.Request;
using GrillBot.Core.Services.Emote.Models.Response;
using Refit;

namespace GrillBot.Core.Services.Emote;

[Service("Emote")]
public interface IEmoteServiceClient : IServiceClient
{
    [Delete("/api/statistics/{guildId}/{emoteId}")]
    Task<int> DeleteStatisticsAsync(string guildId, string emoteId, [Query] string? userId = null, CancellationToken cancellationToken = default);

    [Put("/api/statistics/{guildId}/{sourceEmoteId}/{destinationEmoteId}/merge")]
    Task<MergeStatisticsResult> MergeStatisticsAsync(string guildId, string sourceEmoteId, string destinationEmoteId, CancellationToken cancellationToken = default);

    [Post("/api/statistics/emoteUsersUsage")]
    Task<PaginatedResponse<EmoteUserUsageItem>> GetUserEmoteUsageListAsync(EmoteUserUsageListRequest request, CancellationToken cancellationToken = default);

    [Post("/api/statistics/emoteStatistics")]
    Task<PaginatedResponse<EmoteStatisticsItem>> GetEmoteStatisticsListAsync(EmoteStatisticsListRequest request, CancellationToken cancellationToken = default);

    [Get("/api/emote/supported")]
    Task<List<EmoteDefinition>> GetSupportedEmotesListAsync([Query] string? guildId = null, CancellationToken cancellationToken = default);

    [Delete("/api/emote/supported/{guildId}/{emoteId}")]
    Task DeleteSupportedEmoteAsync(string guildId, string emoteId, CancellationToken cancellationToken = default);

    [Get("/api/emote/{guildId}/{emoteId}/info")]
    Task<EmoteInfo> GetEmoteInfoAsync(string guildId, string emoteId, CancellationToken cancellationToken = default);

    [Get("/api/statistics/count/{guildId}")]
    Task<long> GetStatisticsCountInGuildAsync(string guildId, CancellationToken cancellationToken = default);
}
