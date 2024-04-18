using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Emote.Models.Request;
using GrillBot.Core.Services.Emote.Models.Response;

namespace GrillBot.Core.Services.Emote;

public interface IEmoteServiceClient : IClient
{
    Task<int> DeleteStatisticsAsync(string guildId, string emoteId, string? userId = null);
    Task<MergeStatisticsResult> MergeStatisticsAsync(string guildId, string sourceEmoteId, string destinationEmoteId);
    Task<PaginatedResponse<EmoteUserUsageItem>> GetUserEmoteUsageListAsync(EmoteUserUsageListRequest request);
    Task<PaginatedResponse<EmoteStatisticsItem>> GetEmoteStatisticsListAsync(EmoteStatisticsListRequest request);
    Task<List<EmoteDefinition>> GetSupportedEmotesListAsync(string? guildId = null);
    Task DeleteSupportedEmoteAsync(string guildId, string emoteId);
    Task<EmoteInfo> GetEmoteInfoAsync(string guildId, string emoteId);
    Task<long> GetStatisticsCountInGuildAsync(string guildId);
}
