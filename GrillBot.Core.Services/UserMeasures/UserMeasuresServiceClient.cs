using Cysharp.Web;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.UserMeasures.Models.Dashboard;
using GrillBot.Core.Services.UserMeasures.Models.Measures;
using GrillBot.Core.Services.UserMeasures.Models.User;

namespace GrillBot.Core.Services.UserMeasures;

public class UserMeasuresServiceClient : RestServiceBase, IUserMeasuresServiceClient
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(1);

    public override string ServiceName => "UserMeasures";

    public UserMeasuresServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<List<DashboardRow>> GetDashboardDataAsync()
        => (await ProcessRequestAsync<List<DashboardRow>>(() => HttpMethod.Get.ToRequest("api/dashboard"), _defaultTimeout))!;

    public async Task<int> GetItemsCountOfGuildAsync(string guildId)
        => await ProcessRequestAsync<int>(() => HttpMethod.Get.ToRequest($"api/info/count/{guildId}"), _defaultTimeout);

    public async Task<PaginatedResponse<MeasuresItem>> GetMeasuresListAsync(MeasuresListParams parameters)
        => (await ProcessRequestAsync<PaginatedResponse<MeasuresItem>>(() => HttpMethod.Post.ToRequest("api/measures/list", parameters), _defaultTimeout))!;

    public async Task<UserInfo> GetUserInfoAsync(string guildId, string userId)
        => (await ProcessRequestAsync<UserInfo>(() => HttpMethod.Get.ToRequest($"api/user/{guildId}/{userId}"), _defaultTimeout))!;

    public async Task DeleteMeasureAsync(DeleteMeasuresRequest request)
    {
        var queryParams = WebSerializer.ToQueryString(request);
        await ProcessRequestAsync(() => HttpMethod.Delete.ToRequest($"api/measures?{queryParams}"), _defaultTimeout);
    }
}
