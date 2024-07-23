using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.SearchingService.Models.Request;
using GrillBot.Core.Services.SearchingService.Models.Response;

namespace GrillBot.Core.Services.SearchingService;

public class SearchingServiceClient : RestServiceBase, ISearchingServiceClient
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(1);

    public override string ServiceName => "Searching";

    public SearchingServiceClient(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public async Task<PaginatedResponse<SearchListItem>> GetSearchingListAsync(SearchingListRequest request)
        => (await ProcessRequestAsync<PaginatedResponse<SearchListItem>>(() => HttpMethod.Post.ToRequest("api/items/list", request), _defaultTimeout))!;

    public async Task<List<SearchSuggestion>> GetSuggestionsAsync(string guildId, string channelId)
        => (await ProcessRequestAsync<List<SearchSuggestion>>(() => HttpMethod.Get.ToRequest($"api/items/suggestions/{guildId}/{channelId}"), _defaultTimeout))!;

    public Task RemoveSearchingAsync(long id)
        => ProcessRequestAsync(() => HttpMethod.Delete.ToRequest($"api/items/remove/{id}"), _defaultTimeout);
}
