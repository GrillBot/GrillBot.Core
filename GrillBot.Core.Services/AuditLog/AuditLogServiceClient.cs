using GrillBot.Core.Infrastructure.Auth;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.AuditLog.Models.Request.Search;
using GrillBot.Core.Services.AuditLog.Models.Response;
using GrillBot.Core.Services.AuditLog.Models.Response.Detail;
using GrillBot.Core.Services.AuditLog.Models.Response.Info;
using GrillBot.Core.Services.AuditLog.Models.Response.Info.Dashboard;
using GrillBot.Core.Services.AuditLog.Models.Response.Search;
using GrillBot.Core.Services.AuditLog.Models.Response.Statistics;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;

namespace GrillBot.Core.Services.AuditLog;

public class AuditLogServiceClient : RestServiceBase, IAuditLogServiceClient
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan _infiniteTimeout = Timeout.InfiniteTimeSpan;

    public override string ServiceName => "AuditLog";

    public AuditLogServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory, ICurrentUserProvider currentUser)
        : base(counterManager, httpClientFactory, currentUser) { }

    public async Task<PaginatedResponse<LogListItem>> SearchItemsAsync(SearchRequest request)
        => (await ProcessRequestAsync<PaginatedResponse<LogListItem>>(() => HttpMethod.Post.ToRequest("api/logItem/search", request), _defaultTimeout))!;

    public async Task<Detail?> GetDetailAsync(Guid id)
        => await ProcessRequestAsync<Detail>(() => HttpMethod.Get.ToRequest($"api/logItem/{id}"), _defaultTimeout);

    public async Task<ArchivationResult?> CreateArchivationDataAsync()
        => await ProcessRequestAsync<ArchivationResult>(() => HttpMethod.Post.ToRequest("api/archivation"), _infiniteTimeout);

    public async Task<ApiStatistics> GetApiStatisticsAsync()
        => (await ProcessRequestAsync<ApiStatistics>(() => HttpMethod.Get.ToRequest("api/statistics/api/stats"), _defaultTimeout))!;

    public async Task<AuditLogStatistics> GetAuditLogStatisticsAsync()
        => (await ProcessRequestAsync<AuditLogStatistics>(() => HttpMethod.Get.ToRequest("api/statistics/auditlog"), _defaultTimeout))!;

    public async Task<AvgExecutionTimes> GetAvgTimesAsync()
        => (await ProcessRequestAsync<AvgExecutionTimes>(() => HttpMethod.Get.ToRequest("api/statistics/avgtimes"), _defaultTimeout))!;

    public async Task<InteractionStatistics> GetInteractionStatisticsAsync()
        => (await ProcessRequestAsync<InteractionStatistics>(() => HttpMethod.Get.ToRequest("api/statistics/interactions/stats"), _defaultTimeout))!;

    public async Task<List<UserActionCountItem>> GetUserApiStatisticsAsync(string criteria)
        => (await ProcessRequestAsync<List<UserActionCountItem>>(() => HttpMethod.Get.ToRequest($"api/statistics/api/userstats/{criteria}"), _defaultTimeout))!;

    public async Task<List<UserActionCountItem>> GetUserCommandStatisticsAsync()
        => (await ProcessRequestAsync<List<UserActionCountItem>>(() => HttpMethod.Get.ToRequest("api/statistics/interactions/userstats"), _defaultTimeout))!;

    public async Task<List<JobInfo>> GetJobsInfoAsync()
        => (await ProcessRequestAsync<List<JobInfo>>(() => HttpMethod.Get.ToRequest("api/info/jobs"), _defaultTimeout))!;

    public async Task<int> GetItemsCountOfGuildAsync(ulong guildId)
        => (await ProcessRequestAsync<int>(() => HttpMethod.Get.ToRequest($"api/info/guild/{guildId}/count"), _defaultTimeout))!;

    public async Task<List<DashboardInfoRow>> GetApiDashboardAsync(string apiGroup)
        => (await ProcessRequestAsync<List<DashboardInfoRow>>(() => HttpMethod.Get.ToRequest($"api/dashboard/api/{apiGroup}"), _defaultTimeout))!;

    public async Task<List<DashboardInfoRow>> GetInteractionsDashboardAsync()
        => (await ProcessRequestAsync<List<DashboardInfoRow>>(() => HttpMethod.Get.ToRequest("api/dashboard/interactions"), _defaultTimeout))!;

    public async Task<List<DashboardInfoRow>> GetJobsDashboardAsync()
        => (await ProcessRequestAsync<List<DashboardInfoRow>>(() => HttpMethod.Get.ToRequest("api/dashboard/jobs"), _defaultTimeout))!;

    public async Task<TodayAvgTimes> GetTodayAvgTimes()
        => (await ProcessRequestAsync<TodayAvgTimes>(() => HttpMethod.Get.ToRequest("api/dashboard/todayavgtimes"), _defaultTimeout))!;

    public async Task<StatusInfo> GetStatusInfoAsync()
        => (await ProcessRequestAsync<StatusInfo>(() => HttpMethod.Get.ToRequest("api/diag/status"), _defaultTimeout))!;
}
