﻿using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.AuditLog.Models.Request.CreateItems;
using GrillBot.Core.Services.AuditLog.Models.Request.Search;
using GrillBot.Core.Services.AuditLog.Models.Response;
using GrillBot.Core.Services.AuditLog.Models.Response.Detail;
using GrillBot.Core.Services.AuditLog.Models.Response.Info;
using GrillBot.Core.Services.AuditLog.Models.Response.Info.Dashboard;
using GrillBot.Core.Services.AuditLog.Models.Response.Search;
using GrillBot.Core.Services.AuditLog.Models.Response.Statistics;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;

namespace GrillBot.Core.Services.AuditLog;

public interface IAuditLogServiceClient : IClient
{
    Task CreateItemsAsync(List<LogRequest> requests);
    Task<DiagnosticInfo> GetDiagAsync();
    Task<DeleteItemResponse> DeleteItemAsync(Guid id);
    Task<RestResponse<PaginatedResponse<LogListItem>>> SearchItemsAsync(SearchRequest request);
    Task<Detail?> DetailAsync(Guid id);
    Task<ArchivationResult?> ProcessArchivationAsync();
    Task<ApiStatistics> GetApiStatisticsAsync();
    Task<AuditLogStatistics> GetAuditLogStatisticsAsync();
    Task<AvgExecutionTimes> GetAvgTimesAsync();
    Task<List<StatisticItem>> GetInteractionStatisticsListAsync();
    Task<List<UserActionCountItem>> GetUserApiStatisticsAsync(string criteria);
    Task<List<UserActionCountItem>> GetUserCommandStatisticsAsync();
    Task<List<JobInfo>> GetJobsInfoAsync();
    Task<int> GetItemsCountOfGuildAsync(ulong guildId);
    Task<List<DashboardInfoRow>> GetApiDashboardAsync(string apiGroup);
    Task<List<DashboardInfoRow>> GetInteractionsDashboardAsync();
    Task<List<DashboardInfoRow>> GetJobsDashboardAsync();
    Task<TodayAvgTimes> GetTodayAvgTimes();
    Task<StatusInfo> GetStatusInfoAsync();
}
