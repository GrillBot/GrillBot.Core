using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.AuditLog.Models.Request.Search;
using GrillBot.Core.Services.AuditLog.Models.Response;
using GrillBot.Core.Services.AuditLog.Models.Response.Detail;
using GrillBot.Core.Services.AuditLog.Models.Response.Info;
using GrillBot.Core.Services.AuditLog.Models.Response.Info.Dashboard;
using GrillBot.Core.Services.AuditLog.Models.Response.Search;
using GrillBot.Core.Services.AuditLog.Models.Response.Statistics;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using Refit;

namespace GrillBot.Core.Services.AuditLog;

[Service("AuditLog")]
public interface IAuditLogServiceClient : IServiceClient
{
    [Post("/api/logItem/search")]
    Task<PaginatedResponse<LogListItem>> SearchItemsAsync(SearchRequest request, CancellationToken cancellationToken = default);

    [Get("/api/logItem/{id}")]
    Task<Detail?> GetDetailAsync(Guid id, CancellationToken cancellationToken = default);

    [Post("/api/archivation")]
    Task<ArchivationResult?> CreateArchivationDataAsync(CancellationToken cancellationToken = default);

    [Get("/api/statistics/api/stats")]
    Task<ApiStatistics> GetApiStatisticsAsync(CancellationToken cancellationToken = default);

    [Get("/api/statistics/auditlog")]
    Task<AuditLogStatistics> GetAuditLogStatisticsAsync(CancellationToken cancellationToken = default);

    [Get("/api/statistics/avgtimes")]
    Task<AvgExecutionTimes> GetAvgTimesAsync(CancellationToken cancellationToken = default);

    [Get("/api/statistics/interactions/stats")]
    Task<InteractionStatistics> GetInteractionStatisticsAsync(CancellationToken cancellationToken = default);

    [Get("/api/statistics/api/userstats/{criteria}")]
    Task<List<UserActionCountItem>> GetUserApiStatisticsAsync(string criteria, CancellationToken cancellationToken = default);

    [Get("/api/statistics/interactions/userstats")]
    Task<List<UserActionCountItem>> GetUserCommandStatisticsAsync(CancellationToken cancellationToken = default);

    [Get("/api/info/jobs")]
    Task<List<JobInfo>> GetJobsInfoAsync(CancellationToken cancellationToken = default);

    [Get("/api/info/guild/{guildId}/count")]
    Task<int> GetItemsCountOfGuildAsync(ulong guildId, CancellationToken cancellationToken = default);

    [Get("/api/dashboard/api/{apiGroup}")]
    Task<List<DashboardInfoRow>> GetApiDashboardAsync(string apiGroup, CancellationToken cancellationToken = default);

    [Get("/api/dashboard/interactions")]
    Task<List<DashboardInfoRow>> GetInteractionsDashboardAsync(CancellationToken cancellationToken = default);

    [Get("/api/dashboard/jobs")]
    Task<List<DashboardInfoRow>> GetJobsDashboardAsync(CancellationToken cancellationToken = default);

    [Get("/api/dashboard/todayavgtimes")]
    Task<TodayAvgTimes> GetTodayAvgTimes(CancellationToken cancellationToken = default);

    [Get("/api/diag/status")]
    Task<StatusInfo> GetStatusInfoAsync(CancellationToken cancellationToken = default);

    [Delete("/api/logItem/{id}")]
    Task DeleteItemAsync(Guid id, CancellationToken cancellationToken = default);
}
