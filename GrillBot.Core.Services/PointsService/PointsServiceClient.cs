using System.Net;
using System.Net.Http.Json;
using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.PointsService.Enums;
using GrillBot.Core.Services.PointsService.Models;
using GrillBot.Core.Services.PointsService.Models.Channels;
using GrillBot.Core.Services.PointsService.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace GrillBot.Core.Services.PointsService;

public class PointsServiceClient : RestServiceBase, IPointsServiceClient
{
    public override string ServiceName => "PointsService";

    public PointsServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<RestResponse<PaginatedResponse<TransactionItem>>> GetTransactionListAsync(AdminListRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/admin/list", request, cancellationToken),
            ReadRestResponseAsync<PaginatedResponse<TransactionItem>>,
            async (response, cancellationToken) =>
            {
                if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest) return;
                await EnsureSuccessResponseAsync(response, cancellationToken);
            },
            timeout: TimeSpan.FromSeconds(30)
        );
    }

    public async Task<RestResponse<List<PointsChartItem>>> GetChartDataAsync(AdminListRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/chart", request, cancellationToken),
            ReadRestResponseAsync<List<PointsChartItem>>,
            async (response, cancellationToken) =>
            {
                if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest) return;
                await EnsureSuccessResponseAsync(response, cancellationToken);
            },
            timeout: TimeSpan.FromSeconds(10)
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

    public async Task<RestResponse<List<BoardItem>>> GetLeaderboardAsync(string guildId, int skip, int count, LeaderboardColumnFlag columns, LeaderboardSortOptions sortOptions)
    {
        var queryFields = new Dictionary<string, object>()
        {
            { "skip", skip },
            { "count", count },
            { "columns", (int)columns },
            { "sort", (int)sortOptions }
        };

        var queryParams = string.Join("&", queryFields.Select(o => $"{o.Key}={o.Value}"));

        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/leaderboard/{guildId}?{queryParams}", cancellationToken),
            ReadRestResponseAsync<List<BoardItem>>,
            async (response, cancellationToken) =>
            {
                if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest) return;
                await EnsureSuccessResponseAsync(response, cancellationToken);
            },
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<int> GetLeaderboardCountAsync(string guildId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/leaderboard/{guildId}/count", cancellationToken),
            ReadJsonAsync<int>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<MergeResult?> MergeValidTransctionsAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsync("api/merge", null, cancellationToken),
            ReadJsonAsync<MergeResult>,
            timeout: System.Threading.Timeout.InfiniteTimeSpan
        );
    }

    public async Task<PointsStatus> GetStatusOfPointsAsync(string guildId, string userId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/status/{guildId}/{userId}", cancellationToken),
            ReadJsonAsync<PointsStatus>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<ImagePointsStatus?> GetImagePointsStatusAsync(string guildId, string userId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/status/{guildId}/{userId}/image", cancellationToken),
            async (response, cancellationToken) => response.StatusCode == HttpStatusCode.NotFound ? null : await ReadJsonAsync<ImagePointsStatus>(response, cancellationToken),
            (response, cancellationToken) => response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound ? Task.CompletedTask : EnsureSuccessResponseAsync(response, cancellationToken),
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<ValidationProblemDetails?> TransferPointsAsync(TransferPointsRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/transaction/transfer", request, cancellationToken),
            async (response, cancellationToken) =>
            {
                if (response.StatusCode == HttpStatusCode.NotAcceptable) return null;
                return await DeserializeValidationErrorsAsync(response, cancellationToken);
            },
            async (response, cancellationToken) =>
            {
                if (response.StatusCode is HttpStatusCode.NotAcceptable or HttpStatusCode.BadRequest or HttpStatusCode.OK)
                    return;
                await EnsureSuccessResponseAsync(response, cancellationToken);
            },
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<bool> ExistsAnyTransactionAsync(string guildId, string userId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/transaction/{guildId}/{userId}", cancellationToken),
            ReadJsonAsync<bool>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<StatusInfo> GetStatusInfoAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/diag/status", cancellationToken),
            ReadJsonAsync<StatusInfo>,
            timeout: TimeSpan.FromSeconds(60)
        );
    }

    public async Task<PaginatedResponse<UserListItem>> GetUserListAsync(UserListRequest request)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.PostAsJsonAsync("api/user/list", request, cancellationToken),
            ReadJsonAsync<PaginatedResponse<UserListItem>>,
            timeout: TimeSpan.FromSeconds(30)
        );
    }

    public async Task<int> GetTransactionsCountForGuildActionAsync(string guildId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/transaction/{guildId}/count", cancellationToken),
            ReadJsonAsync<int>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<UserInfo> GetUserInfoAsync(string guildId, string userId)
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/user/{guildId}/{userId}/info", cancellationToken),
            ReadJsonAsync<UserInfo>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public Task<ChannelInfo> GetChannelInfoAsync(string guildId, string channelId)
    {
        return ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/channel/{guildId}/{channelId}/info", cancellationToken),
            ReadJsonAsync<ChannelInfo>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }
}
