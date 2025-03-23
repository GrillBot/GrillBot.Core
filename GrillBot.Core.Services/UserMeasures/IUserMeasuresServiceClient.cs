using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.UserMeasures.Models.Dashboard;
using GrillBot.Core.Services.UserMeasures.Models.Measures;
using GrillBot.Core.Services.UserMeasures.Models.User;
using Refit;

namespace GrillBot.Core.Services.UserMeasures;

[Service("UserMeasures")]
public interface IUserMeasuresServiceClient : IServiceClient
{
    [Get("/api/dashboard")]
    Task<List<DashboardRow>> GetDashboardDataAsync(CancellationToken cancellationToken = default);

    [Get("/api/info/count/{guildId}")]
    Task<int> GetItemsCountOfGuildAsync(string guildId, CancellationToken cancellationToken = default);

    [Post("/api/measures/list")]
    Task<PaginatedResponse<MeasuresItem>> GetMeasuresListAsync(MeasuresListParams parameters, CancellationToken cancellationToken = default);

    [Get("/api/user/{guildId}/{userId}")]
    Task<UserInfo> GetUserInfoAsync(string guildId, string userId, CancellationToken cancellationToken = default);

    [Delete("/api/measures")]
    Task DeleteMeasureAsync([Query] DeleteMeasuresRequest request, CancellationToken cancellationToken = default);
}
