using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.PointsService.Enums;
using GrillBot.Core.Services.PointsService.Models;
using GrillBot.Core.Services.PointsService.Models.Channels;
using GrillBot.Core.Services.PointsService.Models.Users;

namespace GrillBot.Core.Services.PointsService;

public class PointsServiceClient : RestServiceBase, IPointsServiceClient
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan _infiniteTimeout = Timeout.InfiniteTimeSpan;

    public override string ServiceName => "PointsService";

    public PointsServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<PaginatedResponse<TransactionItem>> GetTransactionListAsync(AdminListRequest request)
        => (await ProcessRequestAsync<PaginatedResponse<TransactionItem>>(() => HttpMethod.Post.ToRequest("api/admin/list", request), _defaultTimeout))!;

    public async Task<List<PointsChartItem>> GetChartDataAsync(AdminListRequest request)
        => (await ProcessRequestAsync<List<PointsChartItem>>(() => HttpMethod.Post.ToRequest("api/chart", request), _defaultTimeout))!;

    public async Task<List<BoardItem>> GetLeaderboardAsync(string guildId, int skip, int count, LeaderboardColumnFlag columns, LeaderboardSortOptions sortOptions)
    {
        var query = new Dictionary<string, object>()
        {
            { "skip", skip },
            { "count", count },
            { "columns", (int)columns },
            { "sort", (int)sortOptions }
        };

        var queryParams = string.Join("&", query.Select(o => $"{o.Key}={o.Value}"));
        var uri = $"api/leaderboard/{guildId}?{queryParams}";

        return (await ProcessRequestAsync<List<BoardItem>>(() => HttpMethod.Get.ToRequest(uri), _defaultTimeout))!;
    }

    public async Task<int> GetLeaderboardCountAsync(string guildId)
        => await ProcessRequestAsync<int>(() => HttpMethod.Get.ToRequest($"api/leaderboard/{guildId}/count"), _defaultTimeout);

    public async Task<MergeResult?> MergeValidTransctionsAsync()
        => await ProcessRequestAsync<MergeResult>(() => HttpMethod.Post.ToRequest("api/merge"), _infiniteTimeout);

    public async Task<PointsStatus> GetStatusOfPointsAsync(string guildId, string userId)
        => (await ProcessRequestAsync<PointsStatus>(() => HttpMethod.Get.ToRequest($"api/status/{guildId}/{userId}"), _defaultTimeout))!;

    public async Task<ImagePointsStatus?> GetImagePointsStatusAsync(string guildId, string userId)
        => await ProcessRequestAsync<ImagePointsStatus>(() => HttpMethod.Get.ToRequest($"api/status/{guildId}/{userId}/image"), _defaultTimeout);

    public Task TransferPointsAsync(TransferPointsRequest request)
        => ProcessRequestAsync(() => HttpMethod.Post.ToRequest("api/transaction/transfer", request), _defaultTimeout);

    public Task IncrementPointsAsync(IncrementPointsRequest request)
        => ProcessRequestAsync(() => HttpMethod.Post.ToRequest("api/transaction/increment", request), _defaultTimeout);

    public async Task<bool> ExistsAnyTransactionAsync(string guildId, string userId)
        => await ProcessRequestAsync<bool>(() => HttpMethod.Get.ToRequest($"api/transaction/{guildId}/{userId}"), _defaultTimeout);

    public async Task<StatusInfo> GetStatusInfoAsync()
        => (await ProcessRequestAsync<StatusInfo>(() => HttpMethod.Get.ToRequest("api/diag/status"), _defaultTimeout))!;

    public async Task<PaginatedResponse<UserListItem>> GetUserListAsync(UserListRequest request)
        => (await ProcessRequestAsync<PaginatedResponse<UserListItem>>(() => HttpMethod.Post.ToRequest("api/user/list", request), _defaultTimeout))!;

    public async Task<int> GetTransactionsCountForGuildAsync(string guildId)
        => await ProcessRequestAsync<int>(() => HttpMethod.Get.ToRequest($"api/transaction/{guildId}/count"), _defaultTimeout);

    public async Task<UserInfo> GetUserInfoAsync(string guildId, string userId)
        => (await ProcessRequestAsync<UserInfo>(() => HttpMethod.Get.ToRequest($"api/user/{guildId}/{userId}/info"), _defaultTimeout))!;

    public async Task<ChannelInfo> GetChannelInfoAsync(string guildId, string channelId)
        => (await ProcessRequestAsync<ChannelInfo>(() => HttpMethod.Get.ToRequest($"api/channel/{guildId}/{channelId}/info"), _defaultTimeout))!;
}
