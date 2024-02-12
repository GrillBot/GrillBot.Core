using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.UserMeasures.Models.Dashboard;
using GrillBot.Core.Services.UserMeasures.Models.MeasuresList;
using GrillBot.Core.Services.UserMeasures.Models.User;
using System.Net.Http.Json;
using System.Net;

namespace GrillBot.Core.Services.UserMeasures;

public class UserMeasuresServiceClient : RestServiceBase, IUserMeasuresServiceClient
{
    public override string ServiceName => "UserMeasuresService";

    public UserMeasuresServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<List<DashboardRow>> GetDashboardDataAsync()
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync("api/dashboard", cancellationToken),
            ReadJsonAsync<List<DashboardRow>>,
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

    public async Task<int> GetItemsCountOfGuildAsync(string guildId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/info/count/{guildId}", cancellationToken),
            ReadJsonAsync<int>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }

    public async Task<RestResponse<PaginatedResponse<MeasuresItem>>> GetMeasuresListAsync(MeasuresListParams parameters)
    {
        return await ProcessRequestAsync(
           cancellationToken => HttpClient.PostAsJsonAsync("api/list", parameters, cancellationToken),
           ReadRestResponseAsync<PaginatedResponse<MeasuresItem>>,
           (response, cancellationToken) => response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest ? Task.CompletedTask : EnsureSuccessResponseAsync(response, cancellationToken),
           timeout: TimeSpan.FromSeconds(30)
       );
    }

    public async Task<UserInfo> GetUserInfoAsync(string guildId, string userId)
    {
        return await ProcessRequestAsync(
            cancellationToken => HttpClient.GetAsync($"api/user/{guildId}/{userId}", cancellationToken),
            ReadJsonAsync<UserInfo>,
            timeout: TimeSpan.FromSeconds(10)
        );
    }
}
