using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.Emote.Models.Request;
using GrillBot.Core.Services.Emote.Models.Response;

namespace GrillBot.Core.Services.Emote;

public interface IEmoteServiceClient : IClient
{
    Task<DiagnosticInfo> GetDiagAsync();

    Task<int> DeleteStatisticsAsync(string guildId, string emoteId, string? userId = null);
    Task<MergeStatisticsResult> MergeStatisticsAsync(string guildId, string sourceEmoteId, string destinationEmoteId);
    Task<PaginatedResponse<EmoteUserUsageItem>> GetUserEmoteUsageListAsync(EmoteUserUsageListRequest request);
    Task<RestResponse<PaginatedResponse<EmoteStatisticsItem>>> GetEmoteStatisticsListAsync(EmoteStatisticsListRequest request);

    Task<bool> GetIsEmoteSupportedAsync(string guildId, string emoteId);
    Task<List<string>> GetSupportedEmotesListAsync();
    Task<EmoteInfo> GetEmoteInfoAsync(string guildId, string emoteId);
}
