using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.SearchingService.Models.Request;
using GrillBot.Core.Services.SearchingService.Models.Response;

namespace GrillBot.Core.Services.SearchingService;

public interface ISearchingServiceClient : IClient
{
    Task<PaginatedResponse<SearchListItem>> GetSearchingListAsync(SearchingListRequest request);
    Task<List<SearchSuggestion>> GetSuggestionsAsync(string guildId, string channelId);
    Task RemoveSearchingAsync(long id);
}
