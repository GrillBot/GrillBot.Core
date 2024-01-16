using System.Net;
using System.Net.Http.Json;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.AuditLog.Models.Request.CreateItems;
using GrillBot.Core.Services.AuditLog.Models.Request.Search;
using GrillBot.Core.Services.AuditLog.Models.Response;
using GrillBot.Core.Services.AuditLog.Models.Response.Delete;
using GrillBot.Core.Services.AuditLog.Models.Response.Detail;
using GrillBot.Core.Services.AuditLog.Models.Response.Info;
using GrillBot.Core.Services.AuditLog.Models.Response.Info.Dashboard;
using GrillBot.Core.Services.AuditLog.Models.Response.Search;
using GrillBot.Core.Services.AuditLog.Models.Response.Statistics;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;

namespace GrillBot.Core.Services.AuditLog;

public class AuditLogServiceClient : RestServiceBase, IAuditLogServiceClient
{
    public override string ServiceName => "AuditLog";

    public AuditLogServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task CreateItemsAsync(List<LogRequest> requests)
    {
        await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/logItem", requests, cancellationToken),
            EmptyResponseAsync,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<DiagnosticInfo> GetDiagAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/diag", cancellationToken),
            ReadJsonAsync<DiagnosticInfo>,
            timeout: TimeSpan.FromSeconds(60)
        );
    }

    public async Task<DeleteItemResponse> DeleteItemAsync(Guid id)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.DeleteAsync($"api/logItem/{id}", cancellationToken),
            async (response, cancellationToken) => response.StatusCode == HttpStatusCode.NotFound ? default : await ReadJsonAsync<DeleteItemResponse>(response, cancellationToken),
            (response, cancellationToken) => response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound ? Task.CompletedTask : EnsureSuccessResponseAsync(response, cancellationToken),
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<RestResponse<PaginatedResponse<LogListItem>>> SearchItemsAsync(SearchRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/logItem/search", request, cancellationToken),
            ReadRestResponseAsync<PaginatedResponse<LogListItem>>,
            (response, cancellationToken) => response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest ? Task.CompletedTask : EnsureSuccessResponseAsync(response, cancellationToken),
            timeout: TimeSpan.FromSeconds(30)
        );
    }

    public async Task<Detail?> DetailAsync(Guid id)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/logItem/{id}", cancellationToken),
            async (response, cancellationToken) => response.StatusCode == HttpStatusCode.NotFound ? null : await ReadJsonAsync<Detail>(response, cancellationToken),
            (response, cancellationToken) => response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound ? Task.CompletedTask : EnsureSuccessResponseAsync(response, cancellationToken),
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<ArchivationResult?> ProcessArchivationAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsync("api/archivation", null, cancellationToken),
            ReadJsonAsync<ArchivationResult>,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<ApiStatistics> GetApiStatisticsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/statistics/api/stats", cancellationToken),
            ReadJsonAsync<ApiStatistics>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<AuditLogStatistics> GetAuditLogStatisticsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/statistics/auditlog", cancellationToken),
            ReadJsonAsync<AuditLogStatistics>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<AvgExecutionTimes> GetAvgTimesAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/statistics/avgtimes", cancellationToken),
            ReadJsonAsync<AvgExecutionTimes>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<InteractionStatistics> GetInteractionStatisticsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/statistics/interactions/stats", cancellationToken),
            ReadJsonAsync<InteractionStatistics>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<List<UserActionCountItem>> GetUserApiStatisticsAsync(string criteria)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/statistics/api/userstats/{criteria}", cancellationToken),
            ReadJsonAsync<List<UserActionCountItem>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<List<UserActionCountItem>> GetUserCommandStatisticsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/statistics/interactions/userstats", cancellationToken),
            ReadJsonAsync<List<UserActionCountItem>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<List<JobInfo>> GetJobsInfoAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/info/jobs", cancellationToken),
            ReadJsonAsync<List<JobInfo>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<int> GetItemsCountOfGuildAsync(ulong guildId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/info/guild/{guildId}/count", cancellationToken),
            ReadJsonAsync<int>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<List<DashboardInfoRow>> GetApiDashboardAsync(string apiGroup)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/dashboard/api/{apiGroup}", cancellationToken),
            ReadJsonAsync<List<DashboardInfoRow>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<List<DashboardInfoRow>> GetInteractionsDashboardAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/dashboard/interactions", cancellationToken),
            ReadJsonAsync<List<DashboardInfoRow>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<List<DashboardInfoRow>> GetJobsDashboardAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/dashboard/jobs", cancellationToken),
            ReadJsonAsync<List<DashboardInfoRow>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<TodayAvgTimes> GetTodayAvgTimes()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/dashboard/todayavgtimes", cancellationToken),
            ReadJsonAsync<TodayAvgTimes>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<List<DashboardInfoRow>> GetMemberWarningDashboardAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/dashboard/memberWarning", cancellationToken),
            ReadJsonAsync<List<DashboardInfoRow>>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<StatusInfo> GetStatusInfoAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/diag/status", cancellationToken),
            ReadJsonAsync<StatusInfo>,
            timeout: TimeSpan.FromSeconds(30)
        );
    }

    public async Task<BulkDeleteResponse> BulkDeleteAsync(List<Guid> ids)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PutAsJsonAsync("api/logItem/bulkDelete", ids, cancellationToken),
            ReadJsonAsync<BulkDeleteResponse>,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }
}
