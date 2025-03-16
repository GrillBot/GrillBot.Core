using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.SearchingService.Models.Request;
using GrillBot.Core.Services.SearchingService.Models.Response;
using Refit;

namespace GrillBot.Core.Services.SearchingService;

[Service("Searching")]
public interface ISearchingServiceClient : IServiceClient
{
    [Post("api/items/list")]
    Task<PaginatedResponse<SearchListItem>> GetSearchingListAsync(SearchingListRequest request);

    [Get("api/items/suggestions/{guildId}/{channelId}")]
    Task<List<SearchSuggestion>> GetSuggestionsAsync(string guildId, string channelId);

    [Delete("api/items/remove/{id}")]
    Task RemoveSearchingAsync(long id);
}
